using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMessageCtrl : MonoBehaviour
{
    Vector3 EndPos;
    float Scale = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        EndPos = this.transform.localPosition + (Vector3.up * 300.0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, EndPos, Time.deltaTime * GlobalValue.Game_Speed * 900.0f);
        if (Vector3.Distance(this.transform.localPosition, EndPos) == 0)
        {
            this.GetComponent<Text>().CrossFadeAlpha(0.0f, 0.5f, true);
            Destroy(this.gameObject, 0.5f);
        }

        else
        {
            Scale -= 0.01f;
            this.transform.localScale = new Vector3(Scale, Scale, 1);
        }
    }
}
