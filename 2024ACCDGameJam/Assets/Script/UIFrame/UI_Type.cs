using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Type 
{
    private string path;

    private string name;

    public string Path { get=> path;}
    public string Name { get=> name;}

    /// <summary>
    /// Get UI information
    /// </summary>
    /// <param name="ui_path">The Path of Panel</param>
    /// <param name="ui_name">The Name of Panel</param>
    public UI_Type(string ui_path, string ui_name)
    {
        path= ui_path;
        name= ui_name;
    }
}
