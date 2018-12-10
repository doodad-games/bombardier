using UnityEngine;

public class FloorController : MonoBehaviour
{
    static FloorController _instance;

    static FloorController() =>
        Player.SubGameOver((c) => _instance.enabled = false);

    Vector2 _unitSize;

    void Awake() =>
        _instance = this;

    void Start()
    {
        var numCopies = S.FloorNumCopies;
        _unitSize = S.FloorUnitSize;

        var minRow = -numCopies.x / 2;
        var maxRow = numCopies.x + minRow;
        var minCol = -numCopies.y / 2;
        var maxCol = numCopies.y + minCol;
        for (var row = minRow; row < maxRow; ++row)
        {
            for (var col = minCol; col < maxCol; ++col)
            {
                Instantiate(
                    S.FloorPiece,
                    new Vector3(
                        row * _unitSize.x,
                        col * _unitSize.y,
                        0f
                    ),
                    Quaternion.identity,
                    transform
                );
            }
        }
    }

    void Update()
    {
        var playerPos = Player.Instance.transform.position;
        transform.position = new Vector3(
            Mathf.Floor(playerPos.x / _unitSize.x) * _unitSize.x,
            Mathf.Floor(playerPos.y / _unitSize.y) * _unitSize.y,
            0f
        );
    }
}
