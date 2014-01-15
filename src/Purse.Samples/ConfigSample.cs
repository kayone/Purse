using System;
using System.Configuration;

namespace Purse.Samples
{
public class ConfigSample
{
    private readonly Cache<string, string> _configCache;

    public ConfigSample()
    {
        //Create a new instance of our cache that uses string for both key and value.
        _configCache = new Cache<string, string>();
    }

    public string Get(string key)
    {
        //Tries to get the request value from cache, if it doesn't exist
        //call the function and retrieve the value. (this is generally called a read-through pattern)
        return _configCache.Get(key, () => ReadValue(key));
    }

    public void Set(string key, string value)
    {
        //remove the existing value from cache
        _configCache.Remove(key);

        //write the new value to the backing store
        WriteValue(key, value);

        //update the cache with the new value
        _configCache.Set(key, value);
    }


    private string ReadValue(string key)
    {
        //slow call to read config value from config store.
        throw new NotImplementedException();
    }

    private void WriteValue(string key, string value)
    {
        //call to write config value to config store.
        throw new NotImplementedException();
    }

}
}
