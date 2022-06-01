using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Match3.Graphics;

namespace Match3.Entities
{    
    public class Grid : IGameEntity
    {
        private const int GRID_SIZE = 8;

        private const int TILE_SIZE = 640 / GRID_SIZE;

        private const int Y_OFFSET = 160;

        private const int SWAP_DELAY_MS = 25;

        private const int ADDED_POINTS = 50;

        private static Grid? s_instance = null;        

        private List<Sprite> _charactersSprites;

        private Vector2? _prevSelected;

        private Tile[,] _tiles;

        private ScoreBoard _score;

        private int _ticksSinceNotSuccessfulSwap;        

        private bool _lastSwapSuccessful;

        private Tile _firstSwaper;

        private Tile _secondSwaper;

        private MouseState _mousePrev;        

        protected Grid(List<Texture2D> charactersImg, ScoreBoard score, MouseState mouse)
        {
            _charactersSprites = new List<Sprite>();
            _score = score;            
            _prevSelected = null;
            _lastSwapSuccessful = true;
            _ticksSinceNotSuccessfulSwap = 0;
            _mousePrev = mouse;

            foreach (var character in charactersImg)
            {
                _charactersSprites.Add(new Sprite(character));
            }            

            _tiles = new Tile[GRID_SIZE, GRID_SIZE];

            // filling grid with no match-3 cases
            FillRightWay();

            // default not null
            _firstSwaper = _secondSwaper = _tiles[0, 0];
        }

        public static Grid Initialize(List<Texture2D> charactersImg, ScoreBoard score, MouseState mouse)
        {
            if (s_instance == null)
            {
                s_instance = new Grid(charactersImg, score, mouse);
            }            
            return s_instance;
        } 
        
        public void FillRightWay()
        {
            Random rnd = new Random();

            Sprite? prevLeft = null, prevAbove = null;

            for (int row = 0; row < GRID_SIZE; row++)
            {
                for (int col = 0; col < GRID_SIZE; col++)
                {
                    List<Sprite> possibleCharacters = new List<Sprite>();
                    possibleCharacters.AddRange(_charactersSprites);

                    if (row > 0) prevAbove = _tiles[row - 1, col].GetSprite();
                    if (prevLeft != null) possibleCharacters.Remove(prevLeft);
                    if (prevAbove != null) possibleCharacters.Remove(prevAbove);

                    _tiles[row, col] = new Tile(possibleCharacters[rnd.Next(0, possibleCharacters.Count)], col * TILE_SIZE, row * TILE_SIZE + Y_OFFSET, TILE_SIZE, TILE_SIZE);
                    _tiles[row, col].Deselect();

                    if (col == GRID_SIZE - 1)
                        prevLeft = null;
                    else
                        prevLeft = _tiles[row, col].GetSprite();
                }
            }
        }
        
        private List<Vector2>? FindMatch(int fromX, int fromY, int directionX, int directionY)
        {        
            List<Vector2> result = new List<Vector2>();

            int x = fromX;
            int y = fromY;

            while(x < GRID_SIZE && x > -1 && y < GRID_SIZE && y > -1 && _tiles[fromX, fromY].GetSprite() == _tiles[x, y].GetSprite() && _tiles[x, y].GetSprite() != null)
            {                
                result.Add(new Vector2(x, y));                
                x += directionX;
                y += directionY;
            }            

            if (result.Count > 2)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private void FillNullTiles()
        {
            Random rnd = new Random();
            
            for (int row = GRID_SIZE - 1; row > 0; row--)
            {
                for (int col = GRID_SIZE - 1; col > -1; col--)
                {
                    if (_tiles[row, col].GetSprite() == null)
                    {
                        _score.AddPoints(ADDED_POINTS);
                        int curRow = row - 1;
                        while (curRow > -1 && _tiles[curRow, col].GetSprite() == null)
                        {
                            curRow--;
                        }

                        if (curRow < 0)
                        {                            
                            _tiles[row, col].SetSprite(_charactersSprites[rnd.Next(0, _charactersSprites.Count)]);
                        }
                        else
                        {
                            _tiles[row, col].SetSprite(_tiles[curRow, col].GetSprite());
                            _tiles[curRow, col].SetSprite(null);
                        }                      
                    }
                }
            }

            for (int col = 0; col < GRID_SIZE; col++)
            {
                if (_tiles[0, col].GetSprite() == null)
                {
                    _score.AddPoints(ADDED_POINTS);
                    _tiles[0, col].SetSprite(_charactersSprites[rnd.Next(0, _charactersSprites.Count)]);
                }
            }
        }

        private Vector2? CheckMouseClick()
        {            
            MouseState mouseCurr = Mouse.GetState();
            Vector2? curSelected = null;

            if (_lastSwapSuccessful && _mousePrev.LeftButton == ButtonState.Released && mouseCurr.LeftButton == ButtonState.Pressed)
            {
                var mousePos = mouseCurr.Position;

                for (int row = 0; row < GRID_SIZE; row++)
                {
                    for (int col = 0; col < GRID_SIZE; col++)
                    {
                        if (mousePos.X > col * TILE_SIZE && mousePos.X < col * TILE_SIZE + TILE_SIZE && mousePos.Y > row * TILE_SIZE + Y_OFFSET && mousePos.Y < row * TILE_SIZE + Y_OFFSET + TILE_SIZE)
                        {
                            if (_tiles[row, col].Clicked())
                            {
                                curSelected = new Vector2(row, col);                                
                            }
                            else
                            {
                                _prevSelected = null;
                            }
                        }
                    }
                }
            }

            _mousePrev = mouseCurr;

            return curSelected;
        }

        private void SwapTiles(Vector2? curSelected)
        {
            if (curSelected != null && _prevSelected != null &&
                ((Math.Abs(curSelected.Value.X - _prevSelected.Value.X) < 2 && curSelected.Value.Y == _prevSelected.Value.Y) ||
                (Math.Abs(curSelected.Value.Y - _prevSelected.Value.Y) < 2 && curSelected.Value.X == _prevSelected.Value.X)))
            {
                _firstSwaper = _tiles[(int)_prevSelected.Value.X, (int)_prevSelected.Value.Y];
                _secondSwaper = _tiles[(int)curSelected.Value.X, (int)curSelected.Value.Y];

                _firstSwaper.SwapSprites(_secondSwaper);

                _firstSwaper.Deselect();
                _secondSwaper.Deselect();
                _prevSelected = null;

                bool running;
                _lastSwapSuccessful = false;

                do
                {
                    running = false;
                    List<Vector2> matching = new List<Vector2>();
                    List<Vector2>? cur;

                    for (int row = 0; row < GRID_SIZE; row++)
                    {
                        for (int col = 0; col < GRID_SIZE; col++)
                        {
                            cur = FindMatch(row, col, 1, 0);
                            if (cur != null)
                                matching.AddRange(cur);

                            cur = FindMatch(row, col, 0, 1);
                            if (cur != null)
                                matching.AddRange(cur);

                            cur = FindMatch(row, col, -1, 0);
                            if (cur != null)
                                matching.AddRange(cur);

                            cur = FindMatch(row, col, 0, -1);
                            if (cur != null)
                                matching.AddRange(cur);
                        }
                    }

                    running = matching.Count != 0;

                    _lastSwapSuccessful = running || _lastSwapSuccessful;

                    foreach (var item in matching)
                    {
                        _tiles[(int)item.X, (int)item.Y].SetSprite(null);
                    }

                    FillNullTiles();


                } while (running);
            }
            else if (curSelected != null)
            {
                if (_prevSelected != null)
                    _tiles[(int)_prevSelected.Value.X, (int)_prevSelected.Value.Y].Deselect();
                _prevSelected = curSelected;
            }
        }

        private void CheckNotCorrectSwap()
        {
            if (!_lastSwapSuccessful)
            {
                _ticksSinceNotSuccessfulSwap++;

                if (_ticksSinceNotSuccessfulSwap > SWAP_DELAY_MS)
                {
                    _ticksSinceNotSuccessfulSwap = 0;
                    _firstSwaper.SwapSprites(_secondSwaper);
                    _lastSwapSuccessful = true;
                }
            }           
        }

        public void Update()
        {            
            Vector2? curSelected = CheckMouseClick();
            SwapTiles(curSelected);
        }

        public void Draw(SpriteBatch spriteBatch)
        { 
            for (int row = 0; row < GRID_SIZE; row++)
            {
                for (int col = 0; col < GRID_SIZE; col++)
                {
                    _tiles[row, col].Draw(spriteBatch);                    
                }
            }

            CheckNotCorrectSwap();
        }
    }
}
