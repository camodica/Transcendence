using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class isDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public bool isDragging = false;
    private dropManager dm;

    private double startTime;
    private double currentTime;
    private double triggerTime;
    private bool displayDescription = false;
    public bool displayZoomedView = false;
    private GM gm;

    //original location of a card before it is dragged
    public Transform parentToReturnTo = null;
    //God cares not for As, he cares only for your faith

    public void Start()
    {
        gm = GameObject.Find("GM").GetComponent<GM>();
    }

    public void Update()
    {
        this.currentTime = UnityEngine.Time.time;
        //Debug.Log("Current time is " + this.currentTime);


        if (displayDescription)
        {
            if (this.currentTime >= this.triggerTime && GameObject.Find("Card Hover") == null)
            {
                //Debug.Log("Current time is >= trigger time");

                gm.DisplayDiscription(this.transform.gameObject, true);

                //Debug.Log("Display open for " + this.transform.gameObject.name);
            }
        }
    }

    //incase that the state of the card is needed, this method exists
    public bool isBeingDragged()
    {
        return isDragging;
    }

    public void OnBeginDrag(PointerEventData data) {
		//Debug.Log ("on begin drag");
        isDragging = true;
        this.displayDescription = false;

        if (GameObject.Find("Card Hover") != null)
        {
            Destroy(GameObject.Find("Card Hover"));
        }

        //saves the current parent of the object
        parentToReturnTo = this.transform.parent;
        //sets the objects parent up one level (resetting the grid)
        if (this.transform.parent.CompareTag("Hand"))
        {
            this.transform.SetParent(this.transform.parent.parent);
        }

        if (this.transform.parent.CompareTag("Field"))
        {
            this.transform.SetParent(this.transform.parent.parent.parent);
        }

        this.transform.FindChild("Splash Image").gameObject.GetComponent<Image>().raycastTarget = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        this.GetComponent<Image>().raycastTarget = false;        
    }

    public void OnDrag(PointerEventData data)
    {
		//Debug.Log ("dragging");

        //moves the card to the cursor position
        if (!this.gameObject.GetComponent<Card>().HasBeenPlaced)
        {
            this.transform.position = data.position;

            this.gameObject.tag = "Dragging";
        }
    }

	public void OnEndDrag(PointerEventData data)
    {
        if (!this.gameObject.GetComponent<Card>().HasBeenPlaced)
        {
            //Debug.Log ("on end drag");
            isDragging = false;

            dm = GameObject.Find("GM").GetComponent<dropManager>();

            GameObject draggingCard = GameObject.FindGameObjectWithTag("Dragging");
            GameObject pointerObject = data.pointerCurrentRaycast.gameObject;

            //dm.Drop(data, draggingCard, pointerObject.transform);

            try
            {
                dm.Drop(data, draggingCard, pointerObject.transform);
            }
            catch
            {
                Debug.Log("Nice try, JOHNATHAN!");
            }

            this.transform.SetParent(parentToReturnTo);

            GetComponent<CanvasGroup>().blocksRaycasts = true;
            this.GetComponent<Image>().raycastTarget = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.displayDescription = true;
        this.startTime = UnityEngine.Time.time;
        this.currentTime = UnityEngine.Time.time;
        this.triggerTime = startTime + GameConstants.CARD_HOVER_TIME;
        //Debug.Log("Counter started for " + this.gameObject.name + ".\nStart time is: " + this.startTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.displayDescription = false;

        if (GameObject.Find("Card Hover") != null)
        {
            Destroy(GameObject.Find("Card Hover"));

            //Debug.Log("Display closed for " + this.transform.gameObject.name);
        }

        //Debug.Log("Counter stopped.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //this.transform.FindChild("Function Menu").gameObject.SetActive(true);
    }
}
