using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBarController : MonoBehaviour
{

    private Animator anim;
    public Animator holder;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shop() { SetTrigger("Shop"); }
    public void Gallery() { SetTrigger("Gallery"); }
    public void Main() { SetTrigger("Main"); }

    public void SetTrigger(string name) {
        anim.SetTrigger(name);
        holder.SetTrigger(name);
    }
}
