using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhostController : MonoBehaviour
{
    public GameObject ghost;
    // 0부터 _ghostDelay까지라면 고스트가 생성되지 않음. 그 이후에 생성 후 0으로 초기화
    private float _ghostTimer;

    // 고스트 생성 간격
    private float _ghostDelay;

    // 고스트가 남아있는 시간
    private float _ghostRemainTime;

    public GameObject player;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        _ghostTimer = 0;
        _ghostDelay = 0.02f;

        _ghostRemainTime = 0.1f;
        //player = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        playerController = player.GetComponent<PlayerController>();
        if (playerController._isSpeedForce)
        {
            ghost.SetActive(true);

            if (_ghostTimer <= _ghostDelay)
            {
                _ghostTimer += Time.fixedDeltaTime;
            }
            else
            {
                GameObject newGhost = Instantiate(ghost, player.transform.position, player.transform.rotation);
                SpriteRenderer ghostSprite = newGhost.GetComponent<SpriteRenderer>();
                SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
                ghostSprite.sprite = playerSprite.sprite;
                ghostSprite.flipX = playerSprite.flipX;
                if (playerController._isFacingRight)
                {
                    ghostSprite.flipX = false;
                }
                else
                {
                    ghostSprite.flipX = true;
                }
                _ghostTimer = 0;

                Animator newGhostAnimator = newGhost.GetComponent<Animator>();

                //fade out
                newGhostAnimator.SetFloat("SpeedMultiplier", (1 / _ghostRemainTime));

                Destroy(newGhost, _ghostRemainTime);
            }

        }
        else
        {
            ghost.SetActive(false);
        }


    }
}
