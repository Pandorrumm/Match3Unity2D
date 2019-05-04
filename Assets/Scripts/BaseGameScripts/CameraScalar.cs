using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScalar : MonoBehaviour
{
    private Board board;
    public float cameraOffset;
    public float aspectRatio = 1.777f; //1280/720
    public float padding = 2;// объём помещения кругов, отдаление камеры
    public float xOffset = 1;

	
	void Start ()
    {
        board = FindObjectOfType<Board>();
        if(board != null)
        {
            RepositionCamera(board.width - 1, board.height - 1);
        }
	}
	
	void RepositionCamera(float x, float y) // перемещение камеры
    {
        Vector3 tempPosition = new Vector3(x/2, y/2 + xOffset, cameraOffset);
        transform.position = tempPosition;

        if (board.width >= board.height)
        {
            Camera.main.orthographicSize = (board.width / 2 + padding) / aspectRatio;
        }
        else
        {
            Camera.main.orthographicSize = board.width / 2 + padding;
        }
    }

}
