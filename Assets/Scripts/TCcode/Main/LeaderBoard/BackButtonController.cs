using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonController : MonoBehaviour
{
    public void OnClickBackButton()
    {
        Destroy(gameObject);
    }
}
