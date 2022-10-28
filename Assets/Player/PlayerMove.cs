using UnityEngine;

public class PlayerMove : MonoBehaviour {
    [SerializeField] private protected float speed;

    private Rigidbody2D rb;
    private const string INPUT_AXIS_HORIZONTAL = "Horizontal";

    [SerializeField] private protected float jumpForce;

    private protected float coyoteTimer = 0;
    private protected const float timeToCoyote = 0.025f;

    private enum Animations { Idle, Walk, Jump }
    private Animator animator;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        ControllAnim();
        Move();
        Jump();
    }

    private byte CanJump() {
        RaycastHit2D rayCenter = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, 1 << 7);
        RaycastHit2D rayLeft = Physics2D.Raycast(new Vector2(transform.position.x - 0.3f, transform.position.y), Vector2.down, 0.2f, 1 << 7);
        RaycastHit2D rayRigh = Physics2D.Raycast(new Vector2(transform.position.x + 0.3f, transform.position.y), Vector2.down, 0.2f, 1 << 7);

        if(rayCenter.collider == true || rayLeft.collider == true || rayRigh.collider == true) {
            coyoteTimer = 0;
            return 1;
        }
        else {
            if(coyoteTimer <= timeToCoyote) {
                coyoteTimer += Time.deltaTime;
                return 1;
            }
            else return 0;
        }
    }

    private void Move() {
        float xDirection = Input.GetAxis(INPUT_AXIS_HORIZONTAL);
        rb.velocity = new Vector2(xDirection * speed, rb.velocity.y);

        if(xDirection > 0 && transform.localScale.x < 0) {
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        else if(xDirection < 0 && transform.localScale.x > 0) {
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void Jump() {
        if(Input.GetButtonDown("Jump") && CanJump() == 1) rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ControllAnim() {
        if(CanJump() == 1) {
            if(rb.velocity.x == 0) animator.Play(Animations.Idle.ToString());
            else animator.Play(Animations.Walk.ToString());
        }
        else animator.Play(Animations.Jump.ToString());
    }
}