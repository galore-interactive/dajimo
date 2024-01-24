using UnityEngine;

public class PlayerWeaponRecoilController
{
    private Vector3 _rotationAmountLeft = Vector3.zero;
    private WeaponRecoilConfiguration _configuration;
    private readonly System.Random _randomGenerator;
    private readonly int[] _predictableRandomValues;
    private const int PREDICTABLE_RANDOM_VELUES_SIZE = 100;
    private int _predictableRandomValueIndex;

    public Vector3 RotationAmountLeft => _rotationAmountLeft;
    public int PredictableRandomValueIndex => _predictableRandomValueIndex;

    public PlayerWeaponRecoilController()
    {
        _randomGenerator = new System.Random(113);
        _predictableRandomValues = new int[PREDICTABLE_RANDOM_VELUES_SIZE];
        InitializeRandomValues();
    }

    private void InitializeRandomValues()
    {
        for (int i = 0; i < PREDICTABLE_RANDOM_VELUES_SIZE; i++)
        {
            _predictableRandomValues[i] = _randomGenerator.Next(-100, 100);
        }

        _predictableRandomValueIndex = 0;
    }

    public void SetConfiguration(WeaponRecoilConfiguration newConfiguration)
    {
        _configuration = newConfiguration;
    }

    public void SetRotationAmountLeft(Vector3 newAmount)
    {
        _rotationAmountLeft = newAmount;
    }

    public void SetPredictableRandomValueIndex(int newIndex)
    {
        _predictableRandomValueIndex = newIndex;
    }

    public Vector3 UpdateRecoil(Vector3 currentRotation, float elapsedTime)
    {
        if(_configuration == null)
        {
            return currentRotation;
        }

        _rotationAmountLeft = Vector3.Lerp(_rotationAmountLeft, Vector3.zero, elapsedTime * _configuration.RotationReturnSpeed);
        Vector3 desiredRotation = currentRotation + RotationAmountLeft;
        Vector3 resultRotation = Vector3.Slerp(currentRotation, desiredRotation, elapsedTime * _configuration.RotationalRecoilSpeed);

        return resultRotation;
    }

    public void ApplyRecoil(bool isAiming)
    {
        if(_configuration == null)
        {
            return;
        }

        if (isAiming)
        {
            AddRotationToRecoil(_configuration.RecoilAimingRotation);
        }
        else
        {
            AddRotationToRecoil(_configuration.RecoilRotation);
        }
    }

    private void AddRotationToRecoil(Vector3 newRotationToApply)
    {
        //_rotationAmountLeft += new Vector3(-newRotationToApply.x, Random.Range(-newRotationToApply.y, newRotationToApply.y), 0f);
        _rotationAmountLeft += new Vector3(-newRotationToApply.x, newRotationToApply.y * GetNextRandomMultiplier(), 0f);
    }

    private float GetNextRandomMultiplier()
    {
        float result = _predictableRandomValues[_predictableRandomValueIndex] / 100f;
        IncrementRandomValuesIndex();
        return result;
    }

    private void IncrementRandomValuesIndex()
    {
        _predictableRandomValueIndex++;
        _predictableRandomValueIndex %= PREDICTABLE_RANDOM_VELUES_SIZE;
    }
}
