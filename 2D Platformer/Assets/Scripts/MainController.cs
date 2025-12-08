using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler


{
    public Texture2D hoverCursor;
    private Texture2D defaultCursor;
    public Vector2 hotSpot = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultCursor = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

public void StartLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);

    }

public void QuitGame()
    {
        Application.Quit();
    }
public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Button>())
        {
            Cursor.SetCursor(hoverCursor, hotSpot, CursorMode.Auto);
        }
    }

public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}


