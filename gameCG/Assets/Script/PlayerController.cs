using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    Animator animator;

    //変数定義
    public float flap = 550f;
    public float speed = 10f;
    public Text score;
    public ContactFilter2D filter2d;
    public bool IsStop;
    public int count = 0;

    float direction = 1f;
    float runThreshold = 2.2f;
    float jumpThreshold = 2.0f;
    float stateEffect = 1;
    float runSpeed = 0.5f;
    float nowdirection = 1f;
    bool isGround = true;
    
    int jump = 0;

    AudioSource audio;
    public AudioClip[] sounds;


    string state;
    string prevState;

    // Use this for initialization
    void Start()
    {
        //コンポーネント読み込み
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        audio = GetComponent<AudioSource>();
    }


    // Update is called once per frame

    void Update()
    {
        if (IsStop == true) return;

        Debug.Log(IsStop);
        Move();
        ChangeState();
        ChangeAnimation();
        //Debug.Log(isGround+","+state);

        //地面判定
        isGround = GetComponent<Rigidbody2D>().IsTouching(filter2d);
        Debug.Log(state);
    }


    void Move()
    {
        Vector3 scale = transform.localScale;
        //ジャンプ
        if (Input.GetKeyDown("space"))
        {
            if (isGround==true)
            {
                audio.PlayOneShot(sounds[0]);
                this.rb.AddForce(transform.up * flap);

            }
        }

        //移動(キー判定）
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
           
            scale.x = 1f;
            direction = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            scale.x = -1f;
            direction = -1f;
        }
        else
        {
            direction = 0f;
          
        }

        transform.localScale = scale;

        //移動
        // 左右の移動。一定の速度に達するまではAddforceで力を加え、それ以降はtransform.positionを直接書き換えて同一速度で移動する
        float speedX = Mathf.Abs(this.rb.velocity.x);
        if (speedX < this.runThreshold)
        {
            this.rb.AddForce(transform.right * direction * this.speed * stateEffect); //未入力の場合は key の値が0になるため移動しない
        }
        else
        {
            this.transform.position += new Vector3(runSpeed * Time.deltaTime * direction * stateEffect, 0, 0);
        }

    }

    void ChangeState()
    {
        if (Mathf.Abs(rb.velocity.y) > jumpThreshold)
        {
            isGround = false;
        }

        if (isGround)
        {
            // 走行中
            if (direction != 0)
            {
                state = "RUN";
                //待機状態
            }
            else
            {
                state = "IDLE";
            }
            // 空中にいる場合
        }
        else
        {
            // 上昇中
            if (rb.velocity.y > 0)
            {
                state = "JUMP";
                // 下降中
            }
            else if (rb.velocity.y < 0)
            {
                state = "FALL";
            }
        }
    }



    void ChangeAnimation()
    {
        // 状態が変わった場合のみアニメーションを変更する
        if (prevState != state)
        {
            switch (state)
            {
                case "JUMP":
                    animator.SetBool("jumpup", true);
                    animator.SetBool("jumpdown", false);
                    animator.SetBool("walk", false);
                    animator.SetBool("Stay", false);
                    stateEffect = 0.5f;
                    break;
                case "FALL":
                    animator.SetBool("jumpdown",true);
                    animator.SetBool("jumpup", false);
                    animator.SetBool("walk", false);
                    animator.SetBool("Stay", false);
                    stateEffect = 0.5f;
                    break;
                case "RUN":
                    animator.SetBool("jumpdown", false);
                    animator.SetBool("jumpup", false);
                    animator.SetBool("walk", true);
                    animator.SetBool("Stay", false);
                    stateEffect = 1f;
                    break;
                default:
                    animator.SetBool("walk", false);
                    animator.SetBool("jumpdown", false);
                    animator.SetBool("jumpup", false);
                    animator.SetBool("Stay", true);
                    stateEffect = 1f;
                    break;
            }
            // 状態の変更を判定するために状態を保存してeおく
            prevState = state;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "item")
        {
            audio.PlayOneShot(sounds[1]);
            collision.gameObject.SetActive(false);
            count += 1;
            score.text = ("×"+count.ToString());
            //Debug.Log("hit" + count);
        }
    }

    public void resetscore()
    {
        count = 0;
        score.text = ("×" + count.ToString());
    }

   
}