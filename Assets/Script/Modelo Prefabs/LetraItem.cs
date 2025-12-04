using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetraItem : Item
{
    public string letra;
    public string fondo;
    public string musica;
    public string primeraletra;
    public string restoletra;

    public LetraItem(string letra,string fondo,string musica, string primeraletra,string restoletra) {
        this.letra = letra;
        this.fondo = fondo;
        this.musica = musica;
        this.primeraletra = primeraletra;
        this.restoletra = restoletra;
    }

}
