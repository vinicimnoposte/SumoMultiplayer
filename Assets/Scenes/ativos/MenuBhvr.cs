using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBhvr : MonoBehaviour
{
    [SerializeField] private string _levelName;
    [SerializeField] private GameObject _menuInicialPainel;
    [SerializeField] private GameObject _menuOpcoesPainel;
    public void Play()
    {
        SceneManager.LoadScene(_levelName);
    }
    public void OpenOptions()
    {
        _menuInicialPainel.SetActive(false);
        _menuOpcoesPainel.SetActive(true);
    }
    public void CloseOptions()
    {
        _menuInicialPainel.SetActive(true);
        _menuOpcoesPainel.SetActive(false);
    }
    public void Quit()
    {
        print("Leaving...");
        Application.Quit();     
    }
}
