using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class LoadingScreen : MonoBehaviour{
    private Image _mylogo;
    private bool _loadFinish;
    private bool _endLogo;
    private void Awake(){
         Cursor.lockState=CursorLockMode.Locked;
        _mylogo=GetComponent<Image>();
        _loadFinish=false;
        _endLogo=false;
        _mylogo.color= new Color(_mylogo.color.r,_mylogo.color.g,_mylogo.color.b,0f);

    }

    private void Start(){
        #if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
        #endif

         _loadFinish=true;
    }

    private void Update(){
         if(_loadFinish&&_endLogo){
            SceneManager.LoadSceneAsync("SampleScene");
            //Debug.Log("Ha cargado la escena");
        }
    }

    public void EndAnimationLogo(){
        _endLogo=true;
    }

}




