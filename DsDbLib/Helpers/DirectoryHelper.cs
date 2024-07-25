using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsDbLib.Helpers;

public static class DirectoryHelper
{
    public static string? GetMachineDirectory(string? path, string? machine)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(machine)) 
            return null;
        if (! Directory.Exists(path)) 
            return null;

        var dirs = GetDirectories(path);
        for (var i = 0; i < dirs.GetLength(0); i++)
        {
            if (dirs[i,1].StartsWith(machine))
                return dirs[i,0];
        }
        return null;
    }
    public static string? GetDsDirectory(string? path)
    {
        if ( string.IsNullOrEmpty(path) ) 
            return null;

        var dirs = GetDirectories(path);
        for (int i = 0; i < dirs.GetLength(0); i++)
        {
            if (dirs[i, 1].ToUpper() == "DS")
            {
                return dirs[i,0];
            }
        }
        return null;
    }
    public static string? GetModuleDirectory(string? path, string? moduleName)
    {
        if( string.IsNullOrEmpty(path) || string.IsNullOrEmpty(moduleName) )
            return null;

        var dirs = GetDirectories(path);
        for (int i = 0; i < dirs.GetLength(0); i++)
        {
            if (dirs[i, 1].StartsWith(moduleName))
            {
                return dirs[i,0];
            }
        }
        return null;
    }
    public static string[,] GetDirectories(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return new string[0, 0];
        var dirs = Directory.GetDirectories(path).OrderBy(x => x).ToArray();
        if (dirs == null)
            return new string[0,0];

        var dirArray = new string[dirs.Length, 2];
        for (int i = 0; i < dirs.Length; i++)
        {
            dirArray[i, 0] = dirs[i];
            var dirSplitted = dirs[i].Split('\\');
            dirArray[i, 1] = dirSplitted[dirSplitted.Length - 1];
        }
        return dirArray;
    }
    public static string? GetDirectoryName(this string? path)
    {
        if (string.IsNullOrEmpty(path))
            return null;

        var splittedPath = path.Split('\\');
        return splittedPath[splittedPath.Length -1];
    }
}
