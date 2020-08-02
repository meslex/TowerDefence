

using Enemies;
using UnityEngine;

public class HealthVisualizer : MonoBehaviour
{
	/// <summary>
	/// The object whose X-scale we change to decrease the health bar. Should have a default uniform scale
	/// </summary>
	public Transform healthBar;

	/// <summary>
	/// The object whose X-scale we change to increase the health bar background. Should have a default uniform scale
	/// </summary>
	public Transform backgroundBar;

	/// <summary>
	/// Whether to show this health bar even when it is full
	/// </summary>
	public bool showWhenFull;

	/// <summary>
	/// Camera to face the visualization at
	/// </summary>
	protected Transform m_CameraToFace;

	/// <summary>
	/// Damageable whose health is visualized
	/// </summary>
	public BaseEnemy m_Damageable;

	/// <summary>
	/// Updates the visualization of the health
	/// </summary>
	/// <param name="normalizedHealth">Normalized health value</param>
	public void UpdateHealth(float normalizedHealth)
	{
		Vector3 scale = Vector3.one;

		if (healthBar != null)
		{
			scale.x = normalizedHealth;
			healthBar.transform.localScale = scale;
		}

		if (backgroundBar != null)
		{
			scale.x = 1 - normalizedHealth;
			backgroundBar.transform.localScale = scale;
		}

		SetVisible(showWhenFull || normalizedHealth < 1.0f);
	}

	/// <summary>
	/// Sets the visibility status of this visualiser
	/// </summary>
	public void SetVisible(bool visible)
	{
		gameObject.SetActive(visible);
	}

	/// <summary>
	/// Turns us to face the camera
	/// </summary>
	protected virtual void Update()
	{
		Vector3 direction = m_CameraToFace.transform.forward;
		transform.forward = -direction;
	}

    private void Awake()
    {
		m_Damageable.DamageRecieved += OnHealthChanged;
	}

    private void OnEnable()
    {
		Reset();
	}

	/// <summary>
	/// Caches the main camera
	/// </summary>
	protected virtual void Start()
	{
		m_CameraToFace = UnityEngine.Camera.main.transform;
	}

    private void Reset()
    {
		UpdateHealth(1);

	}

    void OnHealthChanged()
	{
		UpdateHealth(m_Damageable.NormalisedHealth);
	}
}