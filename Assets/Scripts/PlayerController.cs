using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public GameController gameController;

    public float rotationSpeed = 5f;
    public float speed = 5f;

    private bool gameplay = false;
    private bool over = false;

    void Update()
    {
        if (gameplay)
        {
            Vector2 rotTarget = transform.position;

            /*if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = Input.mousePosition;
                rotTarget = Camera.main.ScreenToWorldPoint(mousePos);
                rotTarget.y += 3;
            }*/
            if (Input.touchCount > 0)
            {
                rotTarget = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                rotTarget.y += 3;
            }
            else
            {
                rotTarget.y += 3;
            }

            Vector2 direction = rotTarget - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else
        {
            if (Input.touchCount > 0 && !over)
            {
                gameplay = true;
                gameController.StartGame();
            }

            /*if (Input.GetMouseButton(0) && !over)
            {
                gameplay = true;
                gameController.StartGame();
            }*/
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Scored")
        {
            gameController.Scored(other.gameObject);
            speed = speed + (speed * 0.01f);
        }
        else
        {
            Death();
        }
    }

    private void Death()
    {
        over = true;
        gameplay = false;
        gameController.GameOver();
    }

}
