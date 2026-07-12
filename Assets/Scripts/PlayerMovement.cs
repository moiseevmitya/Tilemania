using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpSpeed = 13f;
    [SerializeField] float firingDelay = 0.75f;
    [SerializeField] [Range(0f, 1f)] float waterMovementMultiplier  = 0.6f;
    [SerializeField] bool canAttack = true;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 20f);
    [SerializeField] Transform gun;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] AudioClip stepsSound, jumpSound, deathSound, attackSound;
     
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    float defaultWaterMovementMultiplier;
    bool isTouchingGround;
    bool isAlive = true;
    bool attackReady = true;
    bool isPaused;
    
    
    
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        defaultWaterMovementMultiplier = waterMovementMultiplier;
    }

    void Update()
    {
        if(!isAlive) return;

        isPaused = pauseMenu.activeSelf;
        
        // проверка контакта с землей
        isTouchingGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        // проверка контанта с лестницей
        bool isTouchingLadder = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        // проверка контанта с водой
        bool isTouchingWater = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Water"));

        if(isPaused) return;
        
        Run();
        FlipSprite();
        ClimbLadder(isTouchingLadder, isTouchingWater);
        InWater(isTouchingLadder, isTouchingWater);
        Die();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive || isPaused) return;
        // чтения ввода движения
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(!isAlive || isPaused) return;
        // проверка на нажатие кнопки и контакт с землей
        if(value.isPressed && isTouchingGround)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed * waterMovementMultiplier);
            AudioSettings.instance.PlaySoundEffect(jumpSound);
        }
    }

    void OnAttack(InputValue value)
    {
        if(!isAlive || !canAttack || isPaused) return;
        // запуск атаки при нажатии
        if(value.isPressed)
        {
            // запуск корутины для ограничение стрельбы по времени
            StartCoroutine(TryAttack());
        }
    }

    void OnPause(InputValue value)
    {
        if(value.isPressed)
        {
            SwitchPauseGame();
        }
    }

    void Run()
    {
        // задаем движение по X, сохранняя Y
        Vector2 playerVelocityX = new Vector2(moveInput.x * moveSpeed * waterMovementMultiplier, myRigidbody.linearVelocityY);
        myRigidbody.linearVelocity = playerVelocityX;
        
        // проверка движения персонажа и переключение анимации бега
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocityX) > Mathf.Epsilon;
        if(hasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        } else
        {
            myAnimator.SetBool("isRunning", false);
        }

        if(hasHorizontalSpeed && isTouchingGround)
        {
            if(!AudioSettings.instance.soundEffectsAudio.isPlaying)
            {
                AudioSettings.instance.PlaySoundEffect(stepsSound);
            }
        }
        
    }

    void ClimbLadder(bool isTouchingLadder, bool isTouchingWater)
    {
        if(isTouchingWater) return;
        
        // если нет контакта с лестницей - возвращаем гравитацию и выходим из метода
        if(!isTouchingLadder)
        {
            myAnimator.SetBool("isClimbing", false);
            myAnimator.speed = 1;
            myRigidbody.gravityScale = gravityScaleAtStart;
            return;
        }

        
        // выключаем гравитацию
        Vector2 playerVelocityY = new Vector2(myRigidbody.linearVelocityX, moveInput.y * climbSpeed);
        myRigidbody.linearVelocity = playerVelocityY;
        myRigidbody.gravityScale = 0;

        // если находимся на лестнице и не касаемся земли - переключаемся на анимацию карабканья
        if(isTouchingLadder && !isTouchingGround)
        {
            myAnimator.SetBool("isClimbing", true);
        }
        else
        {
            myAnimator.SetBool("isClimbing", false);
        }

        // запускаем анимацию при налиции движения и останавливаем, когда стоим
        bool hasVerticalSpeed = Mathf.Abs(myRigidbody.linearVelocityY) > Mathf.Epsilon;
        if(!hasVerticalSpeed && !isTouchingGround)
        {
            myAnimator.speed = 0;
        }
        else
        {
            myAnimator.speed = 1;
        }
    }

    void InWater(bool isTouchingLadder, bool isTouchingWater)
    {
        if(isTouchingLadder) return;

        if(!isTouchingWater)
        {
            waterMovementMultiplier = 1;
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.speed = waterMovementMultiplier;
            return;
        }
        waterMovementMultiplier = defaultWaterMovementMultiplier;
        myRigidbody.gravityScale = gravityScaleAtStart * waterMovementMultiplier;
        myAnimator.speed = waterMovementMultiplier;
    }

    

    void FlipSprite()
    {
        // разворачиваем персонажа по направлению движения
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocityX) > Mathf.Epsilon;
        if(hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocityX), 1f);
        }
        
    }

    void Die()
    {
        // активируем смерть персонажа при контаке с перечисленными слоями
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards", "Acid")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.linearVelocity = deathKick;
            AudioSettings.instance.PlaySoundEffect(deathSound);
            FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }

    // стрельба с задержкой
    private IEnumerator TryAttack()
    {
        if(attackReady)
        {
            attackReady = false;
            Instantiate(bullet, gun.position, gun.rotation);
            AudioSettings.instance.PlaySoundEffect(attackSound);
            yield return new WaitForSeconds(firingDelay);
            attackReady = true;
        }
    }

    private void SwitchPauseGame()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }



}
