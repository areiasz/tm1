using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;

    public GameController gameController;
    public GameController gameController2;

    private float lastSpeedIncreaseScore = 0f;
    private const float speedIncreaseAmount = 2f;

    public float speed;
    public float jumpHeight;
    private float jumpVelocity;
    public float gravity;
    public float horizontalSpeed;

    public float boostAmount = 5f;
    public float boostDuration = 2f;

    private Vector3 defaultColliderCenter;
    private float defaultColliderHeight;

    // Define os limites das lanes
    //private float laneWidth = 4.28f; // Largura total das lanes
    private float[] lanePositions = { -3.88f, 1.40f, 6.65f }; // Posições x das lanes

    // Variáveis para controlar o movimento
    private int currentLaneIndex = 1; // Começa no meio


    private bool jump = false;
    private bool slide = false;
    private bool sprint = false;
    public Animator anim;
    public bool isDead = false;
    private GameController gc;


    void Start()
    {
        GameController[] gameControllers = FindObjectsOfType<GameController>();
        if (gameControllers.Length >= 2)
            //{
            //   //GameController gameController2 = gameControllers[1];
            //}
            Debug.Log(gameControllers.Length);

        gameController = gameControllers[0];
        gameController2 = gameControllers[1];
        //gameController2 = gameControllers[1];
        //Debug.Log("2");
        controller = GetComponent<CharacterController>();
        gc = FindObjectOfType<GameController>();
        lastSpeedIncreaseScore = gameController2.score;
        float highScore = PlayerPrefs.GetFloat("HighScore", 0);
        int coins = PlayerPrefs.GetInt("coins", 0);
        defaultColliderCenter = controller.center;
        defaultColliderHeight = controller.height;
        FindObjectOfType<AudioManager>().PlaySound("MainTheme");
    }

    void Update()
    {

        //Atualização da speed durante o jogo
        if (gameController2.score - lastSpeedIncreaseScore >= 50)
        {
            // Aumenta a velocidade do jogador em 2 unidades
            speed += speedIncreaseAmount;

            // Atualiza o valor do score da última vez que a speed foi aumentada
            lastSpeedIncreaseScore = gameController2.score;
        }

        if (!isDead && gameController2.score > PlayerPrefs.GetFloat("HighScore", 0))
        {
            // Atualiza o recorde atual com o novo score atual
            PlayerPrefs.SetFloat("HighScore", gameController2.score);
            PlayerPrefs.Save(); // Salva os dados imediatamente

            // Notifica o GameController sobre a mudança de recorde
            gameController2.UpdateHighScoreText(gameController2.score);
        }




        Vector3 direction = Vector3.forward * speed;



        // Movimento lateral
        if (!jump && !slide)
        {
            // Define os estados de slide e pulo como falsos para garantir que apenas uma animação seja reproduzida de cada vez
            anim.SetBool("slide", false);
            anim.SetBool("jump", false);

            if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLaneIndex > 0)
            {
                StartCoroutine(MoveToLane(currentLaneIndex - 1));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLaneIndex < lanePositions.Length - 1)
            {
                StartCoroutine(MoveToLane(currentLaneIndex + 1));
            }
        }

        // Pulo
        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // Define os estados de slide e pulo como falsos e verdadeiro, respectivamente, para reproduzir a animação de pulo
                anim.SetBool("slide", false);
                anim.SetBool("jump", true);

                jumpVelocity = jumpHeight;
                jump = true;
            }
            else
            {
                anim.SetBool("jump", false);
                jumpVelocity = 0f;
            }
        }
        else
        {
            // Se não estiver no chão, aplicar a gravidade com base no tempo real
            jumpVelocity -= gravity * Time.deltaTime;

            // Checar se estamos no pico do salto
            if (jump && jumpVelocity <= 0)
            {
                // Mudar o estado de pulo para falso, indicando que estamos começando a cair
                jump = false;
            }
        }

        // Slide
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // Define os estados de slide e pulo como verdadeiro e falso, respectivamente, para reproduzir a animação de slide
            anim.SetBool("slide", true);
            anim.SetBool("jump", false);


            controller.center = new Vector3(controller.center.x, 0.94f, controller.center.z);
            controller.height = 1.86f;
        }
        else
        {
            // Se a tecla de deslize não estiver sendo pressionada, interrompa a animação de deslize
            anim.SetBool("slide", false);

            // Verifica se a animação de deslize terminou
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("sliding"))
            {
                // Se a animação de deslize terminou, chame OnSlideEnd()
                OnSlideEnd();
            }
        }

        direction.y = jumpVelocity;
        controller.Move(direction * Time.deltaTime);
    }

    IEnumerator MoveToLane(int targetLaneIndex)
    {
        float targetX = lanePositions[targetLaneIndex];
        while (Mathf.Abs(transform.position.x - targetX) > 0.1f)
        {
            float moveDirection = Mathf.Sign(targetX - transform.position.x);
            controller.Move(Vector3.right * moveDirection * Time.deltaTime * horizontalSpeed);
            yield return null;
        }
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        currentLaneIndex = targetLaneIndex;
        jump = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica se o jogador colidiu com um obstáculo
        if (other.CompareTag("obstacle") && !isDead)
        {
            StopAllCoroutines();
            anim.SetTrigger("player_die");
            speed = 0;
            jumpHeight = 0;
            horizontalSpeed = 0;
            Invoke("GameOver", 1f);
            isDead = true;
            FindObjectOfType<AudioManager>().StopSound("MainTheme");
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }

        // Verifica se o jogador colidiu com um ring
        if (other.CompareTag("ring"))
        {

            CollectCoin(other.gameObject);

        }

        //Verifica colisão com speedboost
        if (other.CompareTag("speedboost") && !isDead)
        {
            ApplySpeedBoost(boostAmount, boostDuration);
        }
    }

    void GameOver()
    {
        if (gameController2.score > PlayerPrefs.GetFloat("HighScore", 0))
        {
            PlayerPrefs.SetFloat("HighScore", gameController2.score);
            PlayerPrefs.Save(); // Salva os dados imediatamente
        }

        gc.ShowGameOver();
    }

    public void OnSlideEnd()
    {
        // Retorna o collider à posição e tamanho padrão
        controller.center = defaultColliderCenter;
        controller.height = defaultColliderHeight;
    }

    public void ApplySpeedBoost(float boostAmount, float boostDuration)
    {
        if (!sprint)
        {

            StartCoroutine(BoostSpeed(boostAmount, boostDuration));
            anim.SetBool("sprint", true);
            FindObjectOfType<AudioManager>().PlaySound("Boost");
        }
    }
    IEnumerator BoostSpeed(float boostAmount, float boostDuration)
    {
        sprint = true;
        speed += boostAmount;

        yield return new WaitForSeconds(boostDuration);

        speed -= boostAmount;
        sprint = false;
        anim.SetBool("sprint", false);
    }

    //public void SetGameController(GameObject controllerObject)
    //{
    //    GameController controller = controllerObject.GetComponent<GameController>();
    //
    //    if (controller != null)
    //    {
    //        gc = controller;
    //    }
    //}



    /*****Testes********/

    void CollectCoin(GameObject coinObject)
    {
        // Reproduz o som de pegar o anel
        FindObjectOfType<AudioManager>().PlaySound("CoinPickUp");

        // Adiciona uma moeda
        gc.addCoin();

        // Desativa temporariamente a moeda
        StartCoroutine(DisableCoinForSeconds(coinObject, 3f)); // Desativa por 3 segundos (ajuste conforme necessário)
    }

    IEnumerator DisableCoinForSeconds(GameObject coinObject, float seconds)
    {
        coinObject.SetActive(false); // Desativa a moeda

        yield return new WaitForSeconds(seconds);

        coinObject.SetActive(true); // Ativa a moeda novamente após o tempo especificado
    }

    

}
