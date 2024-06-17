using System;
using UnityEngine;
using UnityEngine.InputSystem;


//This class handles rotation of the player camera
[Serializable]
public class MouseLook
{

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;
    public float yRot;
    public float xRot;
    private float xRotNew;
    private float yRotNew;

    public Quaternion m_CharTargetRot;
    public Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;

    float tempx;
    float tempy;
    float tempz;
    float t;

    public void Init(Transform character, Transform camera)
    {
        m_CharTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;
    }


    public void LookRotation(Transform character, Transform camera)
    {
        tempx = 0f;
        t = 0f;

        //yrot xrot values updated by rbfps controller
        xRotNew = xRot * XSensitivity;
        yRotNew = yRot * YSensitivity;

        m_CharTargetRot.x = 0f;
        m_CharTargetRot.z = 0f;
        m_CameraTargetRot.z = 0f;
        m_CameraTargetRot.y = 0f;

        m_CharTargetRot *= Quaternion.Euler(0, yRotNew, 0);


        m_CameraTargetRot *= Quaternion.Euler(-xRotNew, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        character.localRotation = m_CharTargetRot;
        camera.localRotation = m_CameraTargetRot;

        UpdateCursorLock();
    }

    public void LookRotationOverride(Transform character, Transform camera)
    {
        m_CharTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;

        m_CharTargetRot.x = 0f;
        m_CharTargetRot.z = 0f;
        m_CameraTargetRot.z = 0f;
        m_CameraTargetRot.y = 0f;
    }

    public void LookRotationOverideCam(Transform character, Transform camera)
    {
        xRotNew = xRot * XSensitivity;
        yRotNew = yRot * YSensitivity;


        m_CameraTargetRot *= Quaternion.Euler(-xRotNew, yRotNew, 0f);
        m_CameraTargetRot.z = 0f;
        m_CameraTargetRot.y = 0f;
        camera.localRotation = m_CameraTargetRot;


        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);


        UpdateCursorLock();
    }

    //Function that returns the camera to forward rotation, make sure CanRotate is set to false when calling
    public void CamGoBack(Transform character, Transform camera, float speed)
    {

        t += Time.deltaTime * speed;

        if (tempx == 0f)
        {
            tempx = m_CameraTargetRot.x;
            tempy = m_CameraTargetRot.y;
            tempz = m_CameraTargetRot.z;
        }

        m_CameraTargetRot.x = Mathf.Lerp(tempx, 0, t);
        m_CameraTargetRot.y = Mathf.Lerp(tempy, 0, t);
        m_CameraTargetRot.z = Mathf.Lerp(tempz, 0, t);


        camera.localRotation = m_CameraTargetRot;


    }

    public void CamReset(Transform camera)
    {
        m_CameraTargetRot.x = 0;
        m_CameraTargetRot.y = 0;
        m_CameraTargetRot.z = 0;

        camera.localRotation = m_CameraTargetRot;
    }




    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        /*
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        */
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}

