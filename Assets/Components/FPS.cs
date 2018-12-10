using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    Text _text;

    void Awake() =>
        _text = GetComponent<Text>();
    
    void Update() =>
        _text.text = Mathf.Round(Time.timeScale / Time.smoothDeltaTime).ToString();
}