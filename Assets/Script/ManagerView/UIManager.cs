using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject vistaInit;
    
    private Stack<GameObject> viewStack = new Stack<GameObject>();

    private void Awake()
    {
        OpenView(vistaInit);
    }

    private void Start()
    {
        
    }

    public void OpenView(GameObject view)
    {
        if (viewStack.Count > 0)
        {
            viewStack.Peek().SetActive(false); // Ocultamos la vista actual
        }

        view.SetActive(true);
        viewStack.Push(view);
    }

    public void Back()
    {
        if (viewStack.Count > 0)
        {
            var topView = viewStack.Pop();
            topView.SetActive(false);
        }

        if (viewStack.Count > 0)
        {
            viewStack.Peek().SetActive(true); // Mostrar la vista anterior
        }
    }

    public void ResetViews()
    {
        while (viewStack.Count > 0)
        {
            viewStack.Pop().SetActive(false);
        }
    }

}
