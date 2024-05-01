using SnookerScoringSystem.GameplayServices.Interfaces;
using SnookerScoringSystem.Domain;
using SnookerScoringSystem.UseCases.Interfaces;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Maui.Controls;
using System.Runtime.CompilerServices;
using SnookerScoringSystem.UseCases;
using SnookerScoringSystem.Domain.Messages;

namespace SnookerScoringSystem.GameplayServices
{
    // All the code in this file is included in all platforms.

    public partial class CalculateScore : ICalculateScore
    {
        private readonly IUpdatePlayerScoreUseCase _updatePlayerScoreUseCase;
        private readonly IGetPlayerUseCase _getPlayerUseCase;

        public CalculateScore(IUpdatePlayerScoreUseCase updatePlayerScoreUseCase, IGetPlayerUseCase getPlayerUseCase)
        {
            this._updatePlayerScoreUseCase = updatePlayerScoreUseCase;
            this._getPlayerUseCase = getPlayerUseCase;
            RefreshPlayer();
        }

        private enum BallColour
            {
                Redball = 0,
                Yellowball = 1,
                Greenball = 2,
                Brownball = 3,
                Whiteball = 4,
                Blueball = 5,
                Pinkball = 6,
                Blackball = 7,
                PocketHole = 8,
                Nullball = 9
            }

            private enum Fouling
            {
                CueballPocketed,
                FoulStroke
            }

        //Global Variable Initialization
            public List<int> PlayersScores = [ 0, 0 ];
            public List<int> PlayersFouls = [ 0, 0 ];
            private List<Player> Players = new();
            int[] BallScores = { 1, 2, 3, 4, 4, 5, 6, 7 };
            private bool[] IsBallPocketed = { false, false, false, false, false, false, false, false };
            private bool[] DisappearedNearPocket = { false, false, false, false, false ,false ,false };
            private string[] BallNames = { "Red Ball", "Yellow Ball", "Green Ball", "Brown Ball", "White Ball", "Blue Ball", "Pink Ball", "Black Ball" };
            private int DisappearedNearPocketRedBall = 0;
            private int PreviousRedBallAmount = 15;
            private int CurrentPocketOrder = 1;
            private int PlayerTurn = 0;
            private bool Pocketed = false;
            private bool FoulingTurn = false;
            private bool MustHitRed = true;
            private bool PlayerTurnStarted = false;
            private Dictionary<DetectedBall, BallLocation> LastLocationInfo = new();

            private async Task RefreshPlayer()
            {
                Players = await _getPlayerUseCase.ExecuteAsync();
            }

            public void Reset()
            {
                PlayersScores = [0, 0];
                PlayersFouls = [0, 0];
                for (int i = 0; i < 8; i++)
                {
                    IsBallPocketed[i] = false;
                } 
                PreviousRedBallAmount = 15;
                PlayerTurn = 0;
                Pocketed = false;
                FoulingTurn = false;
                MustHitRed = true;
                PlayerTurnStarted = false;
                for (int i = 0; i < 8; i++)
                {
                    DisappearedNearPocket[i] = false;
                } 
                DisappearedNearPocketRedBall = 0;
                LastLocationInfo_test.Clear();
                LastLocationInfo.Clear();
                RefreshPlayer();
            }

            private List<DetectedBall> LastLocationInfo_test = new();
            private double DistanceBetweenXY(double x1, double y1, double x2, double y2)
            {
                return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
            }
            public class GuessedBall
            {
                public DetectedBall cur_ball;
                public DetectedBall last_ball;
                public GuessedBall(DetectedBall cur_ball, DetectedBall last_ball)
                {
                    this.cur_ball = cur_ball;
                    this.last_ball = last_ball;
                }
            }

            //Threshold for the ball location differences.
            private bool notEQ(double tempdouble1, double tempdouble2)
            {
                var tempdouble = tempdouble1 - tempdouble2;
                return tempdouble < -5d || tempdouble > 5d;
            }
            private int last_tick = 0;
        
            public async Task CalculateScoreAsync(List<DetectedBall> detectedBalls)
            {
            if (last_tick != 0)
                    System.Diagnostics.Debug.WriteLine($"time since last executed: {System.Environment.TickCount - last_tick}ms");
                last_tick = System.Environment.TickCount;
            if (LastLocationInfo_test.Count > 0 && detectedBalls.Count > 0)
            {
                // red ball count miss match
                var lastRedBalls = LastLocationInfo_test.Where(ball => ball.ClassId == (int)BallColour.Redball);
                var curRedBalls = detectedBalls.Where(ball => ball.ClassId == (int)BallColour.Redball);
                if (lastRedBalls.Count() != curRedBalls.Count())
                {
                    List<GuessedBall> guessedBalls = new();
                    DetectedBall nearest;
                    foreach (var cur_ball in curRedBalls)
                    {
                        nearest = null; // null should never appear after processing
                        double distance = 9999999999999d;
                        foreach (var last_ball in lastRedBalls)
                        {
                            double cur_distance = DistanceBetweenXY(last_ball.X, last_ball.Y, cur_ball.X, cur_ball.Y);
                            if (cur_distance < distance)
                            {
                                distance = cur_distance;
                                nearest = last_ball;
                            }
                        }
                        if (nearest == null)
                            System.Diagnostics.Debugger.Break(); // ???
                        guessedBalls.Add(new GuessedBall(cur_ball, nearest));
                    }

                    #region check duplicate
                    Dictionary<DetectedBall, int> check_duplicate = new();
                    foreach (var ball in guessedBalls)
                    {
                        if (check_duplicate.ContainsKey(ball.last_ball))
                            check_duplicate[ball.last_ball]++;
                        else
                            check_duplicate.Add(ball.last_ball, 1);
                    }
                    if (check_duplicate.Where(x => x.Value > 1).Count() > 0)
                        ;//System.Diagnostics.Debugger.Break(); // need higher sample per sec
                    #endregion

                    #region check if disappear near pocket
                    //Redballs
                    var ballsMissingFromLast = lastRedBalls.Where(a => guessedBalls.Where(b => b.last_ball == a).Count() == 0);
                    var pockets = LastLocationInfo_test.Where(x => x.ClassId == /* pocket */8);
                    foreach (var Redball in ballsMissingFromLast)
                    {
                        bool disappearNearPocket = false;
                        double nearest_range = 99999999999f;
                        foreach (var pocket in pockets)
                        {
                            double range = DistanceBetweenXY(pocket.X, pocket.Y, Redball.X, Redball.Y);
                            if (range < /* range */ 600d)
                            {
                                System.Diagnostics.Debug.WriteLine($"ball({Redball.X}, {Redball.Y}) within pocket({pocket.X}, {pocket.Y}) range.");
                                DisappearedNearPocketRedBall++;
                                disappearNearPocket = true;
                                break;
                            }
                            if (range < nearest_range)
                            {
                                nearest_range = range;
                            }
                        }
                        if (!disappearNearPocket)
                        {
                            System.Diagnostics.Debug.WriteLine($"ball({Redball.X}, {Redball.Y}) out of pocket range. (nearest {nearest_range})");
                        }
                        //Non Redballs
                        foreach (var ball in LastLocationInfo_test)
                        {
                            if (ball.ClassId > (int)BallColour.Redball && ball.ClassId < (int)BallColour.PocketHole &&
                                !detectedBalls.Any(detectedBall => detectedBall.ClassId == ball.ClassId))
                            {
                                bool disappearNearPocketColoured = false;
                                double nearest_range2 = 99999999999f;
                                foreach (var pocket in pockets)
                                {
                                    double range2 = DistanceBetweenXY(pocket.X, pocket.Y, ball.X, ball.Y);
                                    if (range2 < /* range */ 800d)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"ball({ball.X}, {ball.Y}) within pocket({pocket.X}, {pocket.Y}) range.");
                                        DisappearedNearPocket[ball.ClassId - 1] = true;
                                        disappearNearPocket = true;
                                        break;
                                    }
                                    if (range2 < nearest_range2)
                                    {
                                        nearest_range2 = range2;
                                    }
                                }
                                if (!disappearNearPocketColoured)
                                {
                                    System.Diagnostics.Debug.WriteLine($"ball({ball.X}, {ball.Y}) out of pocket range. (nearest {nearest_range2})");
                                }
                            }
                        }

                        #endregion
                        }
                    }
                }
                LastLocationInfo_test = detectedBalls;

                if (LastLocationInfo.Count != 0)
                {
                    //Player Turn Logic
                    //If any ball moved, then the player turn is started.
                    if (IsBallMoved(detectedBalls, (int)BallColour.Whiteball) && !PlayerTurnStarted)
                    {
                        PlayerTurnStarted = true;
                        FoulingTurn = false;
                    }
                    //If all balls are stopped, the player continue the player turn if the player sucecssfully Pocketed a ball.
                    if (!IsAnyBallMoved(detectedBalls, (int)BallColour.Nullball) && !IsAnyRedBallMoved(detectedBalls) && PlayerTurnStarted)
                    {
                        PlayerTurnStarted = false;
                        /*If no ball are Pocketed, the player turn will switch to next player.
                         * Player 1 = 0, Player 2 = 1
                         */
                        if (!Pocketed)
                        {
                            if (!FoulingTurn)
                            {
                                switch (PlayerTurn)
                                {
                                    case 0:
                                        PlayerTurn = 1;
                                        break;
                                    case 1:
                                        PlayerTurn = 0;
                                        break;
                                }
                            }

                            if (!MustHitRed)
                                MustHitRed = true;
                        }
                        Pocketed = false;
                    }
                    if (AnyBallPockectedEvent(detectedBalls))
                    {
                        Pocketed = true;
                    }
                }
                //Clear all the saved location information from the previous frame and then start saving new location information of the current frame.
                LastLocationInfo.Clear();
                foreach (var Obj in detectedBalls)
                {
                    LastLocationInfo.Add(Obj, new BallLocation(Obj.X, Obj.Y));
                }
                await this._updatePlayerScoreUseCase.ExecuteAsync(PlayersScores[0], PlayersFouls[0], PlayersScores[1], PlayersFouls[1]);
                return;
            }

            private bool AnyBallPockectedEvent(List<DetectedBall> detectedBalls)
            {
                //Specific Case for red ball
                int BallPockected = 0;
                bool RedPocketed = false;
                bool ColoredBallPocketed = false;
                int TotalScores = 0;

                //All ball information in previous frame
                Dictionary<int, int> count_bef = new();
                for (int i = (int)BallColour.Redball; i <= (int)BallColour.Blackball; i++)
                {
                    count_bef.Add(i, 0);
                }
                foreach (var Obj in LastLocationInfo)
                {
                    if (count_bef.ContainsKey(Obj.Key.ClassId))
                    {
                        count_bef[Obj.Key.ClassId]++;
                    }
                }

                //All ball information in current frame
                Dictionary<int, int> count_cur = new();
                for (int i = (int)BallColour.Redball; i <= (int)BallColour.Blackball; i++)
                {
                    count_cur.Add(i, 0);
                }
                foreach (var Obj in detectedBalls)
                {
                    if (count_cur.ContainsKey(Obj.ClassId))
                    {
                        count_cur[Obj.ClassId]++;
                    }
                }

                for (int i = (int)BallColour.Redball; i <= (int)BallColour.Blackball; i++)
                {
                    // Ball Return to table
                    if (count_cur[i] > count_bef[i])
                    {
                        if (i != (int)BallColour.Redball && IsBallPocketed[i] && count_cur[i] > 0)
                        {
                            IsBallPocketed[i] = false;
                        }
                    }
                    // skip whiteball and calculate the ball amount from previous frame and current frame, if more ball in previous frame than current, then a ball is missing (Pocketed)
                    if (count_cur[i] < count_bef[i])
                    {
                        //Redball Pocketed
                        if (i == (int)BallColour.Redball && PreviousRedBallAmount > count_cur[i] && DisappearedNearPocketRedBall > 0)
                        {
                            RedPocketed = true;
                            TotalScores += BallScores[i] * DisappearedNearPocketRedBall;
                            if (MustHitRed)
                            {
                                BallPockected += count_bef[i] - DisappearedNearPocketRedBall;
                                PreviousRedBallAmount--;
                                if (BallScores[i] * (count_bef[i] - count_cur[i]) > 0)
                                {
                                    PlayersScores[PlayerTurn] += BallScores[i] * DisappearedNearPocketRedBall;
                                    WeakReferenceMessenger.Default.Send(new ScoringEventPopupMessage(new ScoringEventData(Players[PlayerTurn].Name + " just pocketed a ", BallNames[i], "! scoring "  + BallScores[i] + " point(s).", i)));
                                }
                            }
                            DisappearedNearPocketRedBall = 0;
                        }

                        // Other colour ball Pocketed

                        if (!IsBallPocketed[i] && i != (int)BallColour.Redball && i != (int)BallColour.Whiteball && (count_cur[i] < 2 && count_bef[i] < 2) && DisappearedNearPocket[i-1] && PreviousRedBallAmount > 0)
                        {
                            ColoredBallPocketed = true;
                            TotalScores += TotalScores += BallScores[i] * (count_bef[i] - count_cur[i]);
                            IsBallPocketed[i] = true;
                            if (!MustHitRed && PreviousRedBallAmount > 0)
                            {
                                BallPockected += count_bef[i] - count_cur[i];
                                if (BallScores[i] * (count_bef[i] - count_cur[i]) > 0)
                                {
                                    PlayersScores[PlayerTurn] += BallScores[i];
                                    WeakReferenceMessenger.Default.Send(new ScoringEventPopupMessage(new ScoringEventData(Players[PlayerTurn].Name + " just pocketed a ", BallNames[i], "! scoring " + BallScores[i] + " point(s).", i)));
                                }
                            }
                        }
                    #region All redball pocketed scoring
                    if (!IsBallPocketed[i] && i != (int)BallColour.Redball && i != (int)BallColour.Whiteball && (count_cur[i] < 2 && count_bef[i] < 2) && DisappearedNearPocket[i - 1] && PreviousRedBallAmount <= 0)
                        {
                            TotalScores += TotalScores += BallScores[i] * (count_bef[i] - count_cur[i]);
                            IsBallPocketed[i] = true;
                            if (i == CurrentPocketOrder)
                            {
                                BallPockected += count_bef[i] - count_cur[i];
                                if (BallScores[i] * (count_bef[i] - count_cur[i]) > 0)
                                {
                                    PlayersScores[PlayerTurn] += BallScores[i];
                                    WeakReferenceMessenger.Default.Send(new ScoringEventPopupMessage(new ScoringEventData(Players[PlayerTurn].Name + " just pocketed a ", BallNames[i], "! scoring " + BallScores[i] + " point(s).", i)));
                                }
                            }
                            else if (i != CurrentPocketOrder && i < 8 && i > 0)
                            {
                                CheckFouling(Fouling.FoulStroke, TotalScores);
                                foreach (var obj in detectedBalls)
                                {
                                    if ( obj.ClassId > 0 && obj.ClassId < 8 && obj.ClassId != CurrentPocketOrder )
                                    {
                                        CurrentPocketOrder = obj.ClassId;
                                        break;
                                    }
                                }
                            }
                        }
                    #endregion

                    // Fouling if a cueball is Pocketed (Cueball Pocketed)
                    if (!IsBallPocketed[i] && i == (int)BallColour.Whiteball && (count_cur[i] < 2 && count_bef[i] < 2))
                        {
                            CheckFouling(Fouling.CueballPocketed, 4);
                            IsBallPocketed[i] = true;
                        }

                        //Fouling (Foul Stroke)

                        if ((ColoredBallPocketed && MustHitRed) || (RedPocketed && !MustHitRed) && PreviousRedBallAmount > 0)
                        {
                            CheckFouling(Fouling.FoulStroke, TotalScores);
                        }

                        if (PreviousRedBallAmount > 0)
                        {
                            if (RedPocketed && MustHitRed)
                            {
                                MustHitRed = false;
                            }
                            else if (ColoredBallPocketed && !MustHitRed)
                            {
                                MustHitRed = true;
                            }
                        }
                        else
                        {
                            MustHitRed = false;
                        }
                    }
                }

                return BallPockected > 0;
            }

        //Detect Specific Ball Movement by comparing the XY saved in previous frame with current frame XY using dictionary with primary key as ball class and secondary key as location struct
        #region BallMovementDetection
        private bool IsBallMoved(List<DetectedBall> detectedBalls, int ClassID)
            {
                foreach (var Obj in detectedBalls)
                {
                    if (Obj.ClassId == ClassID)
                    {
                        foreach (var SavedObjects in LastLocationInfo)
                        {
                            if (SavedObjects.Key.ClassId == ClassID)
                                if (
                                    Obj != null && (
                                    notEQ(Obj.X, SavedObjects.Key.X) ||
                                    notEQ(Obj.Y, SavedObjects.Key.Y)
                                ))
                                {
                                    return true;
                                }
                        }
                    }
                }
                return false;
            }
            //Detect any by comparing the XY saved in previous frame with current frame XY using dictionary with primary key as ball class and secondary key as location struct
            private bool IsAnyBallMoved(List<DetectedBall> detectedBalls, int WhichBalltoIgnore)
            {
                foreach (var Obj in detectedBalls)
                {
                    if (Obj.ClassId > (int)BallColour.Redball && Obj.ClassId <= (int)BallColour.Blackball)
                    {
                        foreach (var SavedObjects in LastLocationInfo)
                        {
                            if (SavedObjects.Key.ClassId > (int)BallColour.Redball && SavedObjects.Key.ClassId <= (int)BallColour.Blackball && SavedObjects.Key.ClassId != WhichBalltoIgnore)
                                if (
                                    Obj != null && Obj.ClassId == SavedObjects.Key.ClassId && (
                                    notEQ(Obj.X, SavedObjects.Key.X) ||
                                    notEQ(Obj.Y, SavedObjects.Key.Y)
                                ))
                                {
                                    return true;
                                }
                        }
                    }
                }
                return false;
            }

            //Detect Redball Movement by comparing the XY saved in previous frame with current frame XY using dictionary with primary key as ball class and secondary key as location struct
            private bool IsAnyRedBallMoved(List<DetectedBall> detectedBalls)
            {
                foreach (var Obj in detectedBalls)
                {
                    if (Obj.ClassId == (int)BallColour.Redball)
                    {
                        bool Moved = true;
                        foreach (var SavedObjects in LastLocationInfo)
                        {
                            if (Obj != null && SavedObjects.Key.ClassId == (int)BallColour.Redball && (
                                !notEQ(Obj.X, SavedObjects.Value.X) &&
                                !notEQ(Obj.Y, SavedObjects.Value.Y)
                            ))
                            {
                                Moved = false;
                            }
                        }
                        if (Moved)
                            return true;
                    }
                }
                return false;
            }
        #endregion
        //Fouling
        #region fouling
        private void CheckFouling(Fouling FoulingType, int Score)
                {
                    FoulingTurn = true;
                    PlayersFouls[PlayerTurn] += 1;
                    switch (FoulingType)
                    {
                        /*Fouling 1: FoulStroke
                         * When a player Pocketed the ball in wrong order.
                         */
                        case Fouling.FoulStroke:
                        WeakReferenceMessenger.Default.Send(new ScoringEventPopupMessage(new ScoringEventData(Players[PlayerTurn].Name + " just commited a  ", "FOUL", "! (Pocketing the ball in wrong order), the other player receives " + Score + " point(s).", 0)));
                        switch (PlayerTurn)
                            {
                                case 0:
                                    PlayerTurn = 1;
                                    break;
                                case 1:
                                    PlayerTurn = 0;
                                    break;
                            }
                            PlayersScores[PlayerTurn] += Score;
                        break;
                        /*Fouling 2: Cueball Pocketed
                         * When a player Pocketed the cueball (White ball).
                         */
                        case Fouling.CueballPocketed:
                        WeakReferenceMessenger.Default.Send(new ScoringEventPopupMessage(new ScoringEventData(Players[PlayerTurn].Name + " just commited a  ", "FOUL", "! (Pocketing the white ball), the other player receives " + Score + " point(s).", 0)));
                        switch (PlayerTurn)
                            {
                                case 0:
                                    PlayerTurn = 1;
                                    break;
                                case 1:
                                    PlayerTurn = 0;
                                    break;
                            }
                            PlayersScores[PlayerTurn] += Score;
                            break;
                    }

                //If the fouling committed increase the opponent's score.
               }
        #endregion
    }
}