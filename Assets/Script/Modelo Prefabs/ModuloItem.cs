using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuloItem: Item
{
    public string nombre;
    public string descripcion;
    public bool terminado;
    public bool status;
    public ModuloItem(string nombre, string descripcion, bool terminado, bool status)
    {
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.terminado = terminado;
        this.status = status;
    }
}
