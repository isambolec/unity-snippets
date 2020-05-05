using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Highlights parts of UI. Create GameObjects:
/// Background (Cover) - SpriteRenderer over full screen, Mask Interactions set to "Visible OutSide Mask"
/// Rect&Circle - SpriteMasks
/// Canvas is set to Screen Space - Camera
/// </summary>
public class UIHighlighter : MonoBehaviour
{
    #region STATIC ACCESS
    public static UIHighlighter Instance;

    void Awake()
    {
        Clear();
        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }
    #endregion

    #region ENUMS
    enum Mode
    {
        None,
        Rect,
        Circle
    }
    #endregion

    public Transform cover;
    public Transform circle;
    public Transform rect;

    private float circleWidthHeightPx = 256f;
    private float rectWidthHeightPx = 64f;

    private Vector3[] corners;
    private float scaleX;
    private float scaleY;
    private Image highlighted;

    private Mode mode;

    public void CircleHighlight(GameObject highlightedObject)
    {
        highlighted = highlightedObject.GetComponent<Image>();

        cover.gameObject.SetActive(true);
        circle.gameObject.SetActive(true);

        mode = Mode.Circle;
    }

    public void RectHighlight(GameObject highlightedObject)
    {
        highlighted = highlightedObject.GetComponent<Image>();

        cover.gameObject.SetActive(true);
        rect.gameObject.SetActive(true);

        mode = Mode.Rect;
    }

    void Update()
    {
        if (highlighted == null)
            return;

        switch (mode)
        {
            case Mode.None:
                return;
            case Mode.Rect:
                UpdateRect();
                break;
            case Mode.Circle:
                UpdateCircle();
                break;
            default:
                break;
        }
    }

    private void UpdateCircle()
    {
        corners = new Vector3[4];
        highlighted.rectTransform.GetWorldCorners(corners);

        scaleX = Vector3.Distance(corners[0], corners[3]) / (circleWidthHeightPx * 0.01f) * 1.1f;
        scaleY = scaleX;

        circle.localScale = new Vector3(scaleX, scaleY, 1f);
        circle.position = highlighted.transform.TransformPoint(highlighted.rectTransform.rect.center);
    }

    private void UpdateRect()
    {
        corners = new Vector3[4];
        highlighted.rectTransform.GetWorldCorners(corners);

        scaleX = Vector3.Distance(corners[0], corners[3]) / (rectWidthHeightPx * 0.01f) * 1.1f;
        scaleY = Vector3.Distance(corners[0], corners[1]) / (rectWidthHeightPx * 0.01f) * 1.1f;

        rect.localScale = new Vector3(scaleX, scaleY, 1f);
        rect.position = highlighted.transform.TransformPoint(highlighted.rectTransform.rect.center);
    }

    public void Clear()
    {
        circle.gameObject.SetActive(false);
        rect.gameObject.SetActive(false);
        cover.gameObject.SetActive(false);
        mode = Mode.None;
        highlighted = null;
    }
}
