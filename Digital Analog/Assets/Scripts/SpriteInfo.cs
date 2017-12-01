using UnityEngine;
using System.Collections;

/// <summary>
/// Script identifying a Sprite with information to be used by the GameManager
/// and CollisionDetection classes.
/// </summary>
public class SpriteInfo : MonoBehaviour
{
    // Fields
    #region Fields
    // Collision boundary fields
    public BoundingShape boundingShape;
    public float radius;

    // Other fields
    private SpriteRenderer spriteRenderer;
    private Vector3 boxSize;
    #endregion


    // Enums
    #region Enums
    /// <summary>
    /// An identifier for the type of bounds that a Sprite uses
    /// </summary>
    public enum BoundingShape
    {
        Box,
        Circle
    }
    #endregion


    // Properties
    #region Properties
    /// <summary>
    /// The minimum X value of this sprite's box bounds
    /// </summary>
    public float MinX
    {
        get { return spriteRenderer.bounds.min.x; }
    }

    /// <summary>
    /// The maximum X value of this sprite's box bounds
    /// </summary>
    public float MaxX
    {
        get { return spriteRenderer.bounds.max.x; }
    }

    /// <summary>
    /// The minimum Y value of this sprite's box bounds
    /// </summary>
    public float MinY
    {
        get { return spriteRenderer.bounds.min.y; }
    }

    /// <summary>
    /// The maximum Y value of this sprite's box bounds
    /// </summary>
    public float MaxY
    {
        get { return spriteRenderer.bounds.max.y; }
    }

    /// <summary>
    /// The radius of this sprite's circular bounds
    /// </summary>
    public float Radius
    {
        get { return radius; }
        set
        {
            if (value >= 0)
            {
                radius = value;
            }
        }
    }

    /// <summary>
    /// The currently active shape of this sprite's bounds
    /// </summary>
    public BoundingShape Shape
    {
        get { return boundingShape; }
        set { boundingShape = value; }
    }

    /// <summary>
    /// The sprite renderer for this sprite.
    /// </summary>
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
            {
                if (GetComponent<SpriteRenderer>() != null)
                {
                    spriteRenderer = GetComponent<SpriteRenderer>();

                    Quaternion rotation = transform.localRotation;
                    gameObject.transform.localRotation
                        = Quaternion.Euler(0, 0, 0);
                    radius = (spriteRenderer.bounds.size.x / 2
                        + spriteRenderer.bounds.size.y / 2) / 2;
                    gameObject.transform.localRotation = rotation;
                }
                else
                {
                    return null;
                }
            }

            return spriteRenderer;
        }
    }

    //public Vector3 BoxSize
    //{
    //    get { return boxSize; }
    //    set { boxSize = value; }
    //}
    #endregion


    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (radius == 0)
        {
            radius = (spriteRenderer.bounds.size.x / 2
                + spriteRenderer.bounds.size.y / 2) / 2;
        }

        boxSize = new Vector3(
            spriteRenderer.bounds.size.x,
            spriteRenderer.bounds.size.y);
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// Draw visual representations of the sprite's bounding shape in the
    /// inspector during gameplay
    /// </summary>
    void OnDrawGizmos()
    {
        if (gameObject != null && boundingShape == BoundingShape.Circle)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        else if (gameObject != null && spriteRenderer != null &&
            boundingShape == BoundingShape.Box)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position,
                spriteRenderer.bounds.size);
        }
    }
}