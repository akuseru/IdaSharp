using System.Collections.Concurrent;
using IdaPro.Native;

namespace IdaPro;

/// <summary>
/// C# bindings for IDA SDK using P/Invoke
/// </summary>
public class Ida
{
    private static Ida? _instance;
    

    public static Ida Instance => _instance ??= new Ida();

    private readonly BlockingCollection<(Func<object>, TaskCompletionSource<object?>)> _collection = new();
    
    private readonly Thread _thread;

    /// <summary>
    /// Initialize ida as library
    /// </summary>
    /// <returns>true if successful</returns>
    private Ida()
    {
        // interactions have to handle on the same thread. 
        _thread = new Thread(() =>
        {
            var _alive = 255;
            try
            {
                _alive = Libidalib.init_library(0, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (_alive != 0)
            {
                Console.WriteLine("Thread Failed");
                throw new Exception($"init_library() failed with error: {_alive}");
            }
            
            // #if DEBUG
            Console.WriteLine("Enable debug messages.");
            Libidalib.enable_console_messages(true);
            // #endif

            while (_collection.TryTake(out var p, -1))
            {
                var (action, tcs) = p;
                try
                {
                    Console.WriteLine("Trying to invoke action");
                    var res = action.Invoke();
                    tcs.SetResult(res);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            }
            Console.WriteLine("Ida thread exiting.");
        })
        {
            IsBackground = true,
            Name = "Ida Database Process"
        };
        _thread.Start();
    } 
    
    public bool IsActive => _thread.IsAlive;

    /// <summary>
    /// If the database did not exist, a new database will be created and
    /// the input file will be loaded
    /// </summary>
    /// <param name="database">the file name to be loaded</param>
    /// <param name="auto">if set to true, library will run also auto analysis</param>
    /// <param name="args">optional arguments, respecting IDA's command-line arguments format</param>
    /// <returns>true if successfully opened</returns>
    public async Task<bool> OpenDatabase(string database, bool auto, string? args = null)
    {
        var tcs = new TaskCompletionSource<object?>();
        
        _collection.Add((() => Libidalib.open_database(database, auto, args), tcs));
        
        var res = await tcs.Task;
        
        if (res is int ret)
        {
            return ret == 0;
        }
        return false;
    }

    /// <summary>
    /// Close the current database
    /// </summary>
    /// <returns></returns>
    public async Task CloseDatabase(bool save)
    {
        var tcs = new TaskCompletionSource<object?>();
        _collection.Add((() =>
        {
            Libidalib.close_database(save);
            return true;
        }, tcs));
        
        await tcs.Task;
    }


    /// <summary>
    /// Process everything in the queues and return true.
    /// </summary>
    /// <returns>false if the user clicked cancel. (the wait box must be displayed by the caller if desired)</returns>
    public async Task<bool> Wait()
    {
        var tcs = new TaskCompletionSource<object?>();
        _collection.Add((() => Libida.auto_wait(),
            tcs));
        var res = await tcs.Task;
        if (res is bool ret)
        {
            return ret;
        }
        return false;
    }


    /// <summary>
    /// Creates an enum from the set of data.
    /// </summary>
    /// <param name="name">name of the enum</param>
    /// <param name="data">Enum to create</param>
    /// <param name="size"># of bytes the enum takes</param>
    public async Task CreateEnum(string name, Dictionary<string, int> data, int size)
    {
        
    }
}