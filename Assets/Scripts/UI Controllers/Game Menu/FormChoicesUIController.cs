using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class FormChoicesUIController : MonoBehaviour {
    private Canvas canvas;

    private void Awake() => canvas = GetComponent<Canvas>();

    public void ChooseForm(int form) {
        canvas.enabled = false;

        FormType formType = (FormType)form;

        GameManager.Instance.StartRound(formType);
    }

    public void EnableCanvas() {
        canvas.enabled = true;
    }
}
