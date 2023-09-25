using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBonusGlitter : MonoBehaviour
{
    /*
        Limited pool structure, where the value is iterated on and reset upon spins.
        List of Sprites Stored in Roulette.cs
    */


    public List<Image> pool;

    protected int currentVal;

    // Base no number Experience Sprite
    public Sprite expBase;

    public GameObject expBarPos;
    public GameObject bonusPos;

    public Animator expAnim;
    public Animator bonusAnim;




    public void ResetPool() {
        currentVal = 0;
        pool.ForEach(n => {
            n.gameObject.SetActive(false);
            n.transform.position = Vector3.zero;   
            n.transform.localScale = Vector3.one; 
        });
    }


    public void UseGlitter(Vector3 pos, Sprite usableSpr, bool exp) {
        Image currImg = pool[currentVal];
        currentVal++;

        if(currentVal >= pool.Count) currentVal = 0;

        currImg.sprite = usableSpr;
        currImg.transform.position = pos;
        currImg.gameObject.SetActive(true);

        StartCoroutine(MoveObject(currImg.gameObject, exp ? expBarPos.transform.position : bonusPos.transform.position, exp ? expAnim : bonusAnim));
    }


    IEnumerator MoveObject(GameObject g, Vector3 endPos, Animator finale) {
        Vector3 startPos = g.transform.position;
        
        Debug.Log((g.transform.position-endPos).magnitude);
        float time = 0;
        while((g.transform.position-endPos).magnitude > 6f) {
            g.transform.position = Vector3.Lerp(startPos, endPos, time);
            g.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time);
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }

        g.SetActive(false);
        finale.SetTrigger("Play");
    }


}
