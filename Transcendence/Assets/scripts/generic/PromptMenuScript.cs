using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PromptMenuScript : MonoBehaviour {
    public Canvas canvas;
    public string mutation;
    public Button button1;
    public Button button2;
    public Button button3;
    public Text description;

    // Use this for initialization
    void Start()
    {
        setMutation("unassigned");
        layoutDuo();
        setLeftButtonText("text1");
        setRightButtonText("text2");
        setMiddleButtonText("text3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Change prompt to format |  [button3]  |
    public void layoutSingle()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(true);
    }
    //Change prompt to format |[button1]  [button2]|
    public void layoutDuo()
    {
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        button3.gameObject.SetActive(false);
    }
    public void setValues()
    {

    }
    public void setMutation(string mutation)
    {
        this.mutation = mutation;
    }
    public void setDescription(string str)
    {
        description.text = str;
    }
    public void setLeftButtonText(string str)
    {
        button1.transform.FindChild("Text").GetComponent<Text>().text = str;
    }
    public void setRightButtonText(string str)
    {
        button2.transform.FindChild("Text").GetComponent<Text>().text = str;
    }
    public void setMiddleButtonText(string str)
    {
        button3.transform.FindChild("Text").GetComponent<Text>().text = str;
    }
    //show or hide the entire prompt
    public void isVisible(bool state)
    {
        canvas.enabled = state;
    }
    //moves prompt to center of screen
    public void moveToCenter()
    {
        canvas.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
    }
    //public void setValues(string mutation, string description, string leftButton, string rightButton, delegate callback, delegate callback2);
    public void clear() {
        moveToCenter();
        setMutation("blank");
        setDescription("description");
        setLeftButtonText("button1");
        setRightButtonText("button2");
        setMiddleButtonText("button3");
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
    }


}
