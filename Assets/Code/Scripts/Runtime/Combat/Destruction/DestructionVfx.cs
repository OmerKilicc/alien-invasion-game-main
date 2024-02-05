using Euphrates;
using UnityEngine;

[RequireComponent(typeof(IDestruction))]
public class DestructionVfx : MonoBehaviour
{
    const string CLEANUP_TIMER = "clean_debris_";
    const float DEBRY_ANIM_DURATON = 2f;
    IDestruction _destruction;

    [SerializeField] string _vfxName;
    [SerializeField] string _soundName;

    [Space]
    [SerializeField] VFXChannelSO _vfxChannel;
    [SerializeField] SoundChannelSO _soundChannel;

    [Space]
    [SerializeField] GameObject _visual;
    [SerializeField] GameObject _pieceHolder;
    [SerializeField] float _explosionForceOnPieces = 100f;
    [SerializeField] FloatSO _cleanupTime;

    private void Awake()
    {
        _destruction = GetComponent<IDestruction>();
    }

    private void OnEnable()
    {
        _destruction.OnDestructed += StartEffect;
    }

    private void OnDisable()
    {
        _destruction.OnDestructed -= StartEffect;
    }

    public void StartEffect()
    {
        _vfxChannel.PlayVFX(_vfxName, transform.position);
        _soundChannel.Play(_soundName);

        _visual?.SetActive(false);

        if (_pieceHolder == null)
            return;

        _pieceHolder.SetActive(true);

        Rigidbody[] rbs = _pieceHolder.GetComponentsInChildren<Rigidbody>(false);

        if (rbs == null || rbs.Length == 0)
            return;

        void DisableRBs()
        {
            if (rbs == null)
                return;

            for (int i = 0; i < rbs.Length; i++)
            {
                if (rbs[i] == null)
                    continue;

                GameObject piece = rbs[i].gameObject;
                piece.transform.DoScale(Vector3.zero, DEBRY_ANIM_DURATON, Ease.InElastic, null, () =>
                {
                    if (piece != null)
                        piece.SetActive(false);
                });
            }
        }

        for (int i = 0; i < rbs.Length; i++)
        {
            Rigidbody rb = rbs[i];

            rb.isKinematic = false;
            Vector3 force = Random.insideUnitSphere * _explosionForceOnPieces;

            rb.AddForce(force, ForceMode.Impulse);
        }

        GameTimer.CreateTimer(CLEANUP_TIMER + gameObject.GetInstanceID(), _cleanupTime, DisableRBs);
    }
}
