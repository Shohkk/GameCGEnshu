using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamecontroller : MonoBehaviour
{
    enum GAMEMODE
    {//enum(列挙体)
        TITLE,
        PLAY,
        END
    };

    GAMEMODE nowmode; //現在のゲームモード(変数)

    public Transform titleimage; //Unityのタイトルイメージを取得
    public PlayerController player; //Unityのプレイヤーを取得
    public Transform GoalLabel; //Unityのゴールラベルを取得
    public Transform endimage; //Unityのエンドイメージを取得
    public Transform esa; //Unityのアイテムグループを取得

    // Use this for initialization
    void Start()
    {
        nowmode = GAMEMODE.TITLE; //初期状態のゲームモード(タイトル)
        titleimage.gameObject.SetActive(true); //タイトルイメージを表示する

        //停止状態
        

    }

    // Update is called once per frame
    void Update()
    {
        switch (nowmode)
        { //スイッチ形式、○○だったら××せよ
            case GAMEMODE.TITLE: //タイトル画面で動くプログラム

                if (Input.GetButtonDown("Jump"))
                { //ジャンプボタン押したら
                    nowmode = GAMEMODE.PLAY; //本編へ移動する
                    titleimage.gameObject.SetActive(false); //タイトルイメージを消す


                }
                break;


            case GAMEMODE.PLAY:  //ゲーム本編中で動くプログラム

                if (player.transform.position.x > GoalLabel.position.x)
                { //プレイヤーのX座標がゴールラベルより大きくなったら

                    nowmode = GAMEMODE.END; //エンド画面へ移動する
                    //停止状態を

                    endimage.gameObject.SetActive(true); //エンドイメージを表示する
                }


                break;

            case GAMEMODE.END:  //エンド画面で動くプログラム

                if (Input.GetButtonDown("Jump"))
                { //ジャンプボタン押したら
                    nowmode = GAMEMODE.TITLE; //本編へ移動する
                    titleimage.gameObject.SetActive(true); //タイトルイメージを表示
                    endimage.gameObject.SetActive(false); //エンドイメージを消す

                    for (int i = 0; i < esa.childCount; ++i)
                    { //for文(繰り返し)、esa内のアイテムをあるだけ取得
                        esa.GetChild(i).gameObject.SetActive(true); //取得したiのアイテムを表示する
                    }

                    //プレイヤーのスクリプトにあるリセットを呼び出す
                }

                break;
        }
    }
}
