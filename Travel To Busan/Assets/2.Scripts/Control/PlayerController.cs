using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 입력을 받아 처리하는 클래스
/// ex) 앞으로 이동 -> 걷기 애니메이션과 playerTr의 좌표 수정
/// ex) 피격당함 -> DataManager.instance.playerInfo의 체력 감소
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Public
    public Transform playerTr;
    public Rigidbody2D playerRb;
    public SpriteRenderer playerRender;
    public Animator playerAnimator;
    public BoxCollider2D hitBox;
    public BoxCollider2D attackBox;

    public GameConst.eSpriteDirection playerSprite;
    public PlayerStateControl playerState;
    #endregion

    #region Private
    private bool doesJumping = false;
    #endregion

    private void Start()
    {
        playerState = new PlayerStateControl(playerAnimator);
    }

    private void Update()
    {
        Control();   
    }

    public void Control()
    {
        float horAxis = Input.GetAxis("Horizontal");
        float verAxis = Input.GetAxis("Vertical");

        horAxis = horAxis > 0.5f ? 1 : (horAxis < -0.5f ? -1 : 0);
        verAxis = verAxis > 0.5f ? 1 : (verAxis < -0.5f ? -1 : 0);

        //  앉기
        if (Input.GetKey(KeyCode.S))
            playerState.Sit();
        //  좌우 Translate
        else if (Input.GetKey(KeyCode.A))
        {
            Vector3 dir = new Vector3(-DataManager.Instance.PlayerInfo.Speed, 0, 0);
            playerTr.Translate(dir);
            playerState.Run();

            switch (playerSprite)
            {
                case GameConst.eSpriteDirection.Left:
                    playerRender.flipX = false;
                    break;
                case GameConst.eSpriteDirection.Right:
                    playerRender.flipX = true;
                    break;
                default: break;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Vector3 dir = new Vector3(DataManager.Instance.PlayerInfo.Speed, 0, 0);
            playerTr.Translate(dir);
            playerState.Run();

            switch (playerSprite)
            {
                case GameConst.eSpriteDirection.Left:
                    playerRender.flipX = true;
                    break;
                case GameConst.eSpriteDirection.Right:
                    playerRender.flipX = false;
                    break;
                default: break;
            }
        }

        //  공격
        if (Input.GetKey(KeyCode.Space))
            playerState.Attack();

        //  점프
        if (Input.GetKey(KeyCode.W) && !doesJumping)
        {
            playerRb.AddForce(Vector2.up * DataManager.Instance.PlayerInfo.JumpForce, ForceMode2D.Impulse);
            doesJumping = true;
            playerState.Jump();
            return;
        }
        //  추락
        else if (playerRb.velocity.y < -0.1f)
        {
            doesJumping = true;
            playerState.Fall();
            return;
        }
        //  착지
        else if (doesJumping && playerRb.velocity.y < 0.1f)
        {
            doesJumping = false;
            playerState.Idle();
            return;
        }

        if (!Input.anyKey)
            playerState.Idle();
    }
}
