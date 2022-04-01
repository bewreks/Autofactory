using UnityEngine;

public static class SpriteHelper
{
	public static Sprite GetBlankSprite(int width, int height)
	{
		var ntext = new Texture2D(width, height);

		for (var x = 0; x < ntext.width; x++)
		{
			for (var y = 0; y < ntext.height; y++)
			{
				ntext.SetPixel(x, y, Color.white);
			}
		}

		return Sprite.Create(ntext, new Rect(0, 0, ntext.width, ntext.height), Vector2.one * 0.5f);
	} 
}