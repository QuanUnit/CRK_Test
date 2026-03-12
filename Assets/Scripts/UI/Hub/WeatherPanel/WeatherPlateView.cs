using Services.Weather;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hub.WeatherPanel
{
    public class WeatherPlateView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _temperature;
        [SerializeField] private string _temperatureFormat;

        public void SetView(float temperature, Texture2D iconTexture)
        {
            int temperatureAsInt = (int)temperature;
            _temperature.text = string.Format(_temperatureFormat, temperatureAsInt);

            if (iconTexture != null)
            {
                _icon.sprite = CreateSpriteFrom(iconTexture);
            }
        }

        private Sprite CreateSpriteFrom(Texture2D texture)
        {
            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect
            );
        }
    }
}