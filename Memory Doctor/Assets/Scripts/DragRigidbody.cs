using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DragRigidbody : MonoBehaviour
{
  public float force = 600;
	public float damping = 6;
	
	Transform jointTrans;
	float dragDepth;

    public float verticalDisplacement;

	void OnMouseDown ()
	{
		HandleInputBegin (Input.mousePosition);
	}
	
	void OnMouseUp ()
	{
		HandleInputEnd (Input.mousePosition);
	}
	
	void OnMouseDrag ()
	{
		HandleInput (Input.mousePosition);
	}
	
	public void HandleInputBegin (Vector3 screenPosition)
	{
		var ray = Camera.main.ScreenPointToRay (screenPosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.transform.gameObject.layer == LayerMask.NameToLayer ("Interactive")) {
                transform.position = new Vector3(transform.position.x, transform.position.y + verticalDisplacement, transform.position.z);
				dragDepth = CameraPlane.CameraToPointDepth (Camera.main, hit.point + new Vector3(0,verticalDisplacement,0));
				jointTrans = AttachJoint (hit.rigidbody, hit.point + new Vector3(0,verticalDisplacement,0));
			}
		}
	}
	
	public void HandleInput (Vector3 screenPosition)
	{
		if (jointTrans == null)
			return;
		var worldPos = Camera.main.ScreenToWorldPoint (screenPosition);
        var cameraPosition = CameraPlane.ScreenToWorldPlanePoint (Camera.main, dragDepth, screenPosition);
		jointTrans.position = new Vector3(cameraPosition.x, jointTrans.position.y, cameraPosition.z);
	}
	
	public void HandleInputEnd (Vector3 screenPosition)
	{
        if (jointTrans == null)
			return;
		Destroy (jointTrans.gameObject);
	}
	
	Transform AttachJoint (Rigidbody rb, Vector3 attachmentPosition)
	{
		GameObject go = new GameObject ("Attachment Point");
		go.hideFlags = HideFlags.HideInHierarchy; 
		go.transform.position = attachmentPosition + new Vector3(0,0.5f,0);
		
		var newRb = go.AddComponent<Rigidbody> ();
		newRb.isKinematic = true;
		
		var joint = go.AddComponent<ConfigurableJoint> ();
		joint.connectedBody = rb;
		joint.configuredInWorldSpace = true;
		joint.xDrive = NewJointDrive (force, damping);
		joint.yDrive = NewJointDrive (force, damping);
		joint.zDrive = NewJointDrive (force, damping);
		joint.slerpDrive = NewJointDrive (force, damping);
		joint.rotationDriveMode = RotationDriveMode.Slerp;
		
		return go.transform;
	}
	
	private JointDrive NewJointDrive (float force, float damping)
	{
		JointDrive drive = new JointDrive ();
		drive.mode = JointDriveMode.Position;
		drive.positionSpring = force;
		drive.positionDamper = damping;
		drive.maximumForce = Mathf.Infinity;
		return drive;
	}
}