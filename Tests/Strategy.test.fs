namespace Tests

open NUnit.Framework
open FsUnit

open RouletteStrategy.Entities

[<Category("Strategy")>]
module Strategy = 

    [<Test>]
    let ``getArea`` () =
        RouletteStrategy.Strategy.getArea 1 |> should equal Area.D1
        RouletteStrategy.Strategy.getArea 12 |> should equal Area.D1

        RouletteStrategy.Strategy.getArea 13 |> should equal Area.D2
        RouletteStrategy.Strategy.getArea 24 |> should equal Area.D2

        RouletteStrategy.Strategy.getArea 25 |> should equal Area.D3
        RouletteStrategy.Strategy.getArea 36 |> should equal Area.D3

    [<Test>]
    let ``Strategy_2 <when> D1 not appeared <then> suggests D2 and D3`` () =
        RouletteStrategy.Strategy.strategy_2 [24;36;24;36] 4
        |> should equal Bet.D2_D3

        RouletteStrategy.Strategy.strategy_2 [24;36;24;36; 12] 4
        |> should equal Bet.D2_D3

        RouletteStrategy.Strategy.strategy_2 [24;36;24;36; 24] 4
        |> should equal Bet.D2_D3

        RouletteStrategy.Strategy.strategy_2 [24;36;24;36; 36] 4
        |> should equal Bet.D2_D3

    [<Test>]
    let ``Strategy_2 <when> D2 not appeared <then> suggests D1 and D3`` () =
        RouletteStrategy.Strategy.strategy_2 [12;36;12;36;12] 4
        |> should equal Bet.D1_D3 
       
        RouletteStrategy.Strategy.strategy_2 [12;36;12;36; 12] 4
        |> should equal Bet.D1_D3 

        RouletteStrategy.Strategy.strategy_2 [12;36;12;36; 24] 4
        |> should equal Bet.D1_D3 

        RouletteStrategy.Strategy.strategy_2 [12;36;12;36; 36] 4
        |> should equal Bet.D1_D3 

    [<Test>]
    let ``Strategy_2 <when> D3 not appeared <then> suggests D1 and D2`` () =
        RouletteStrategy.Strategy.strategy_2 [12;24;12;24] 4 
        |> should equal Bet.D1_D2

        RouletteStrategy.Strategy.strategy_2 [12;24;12;24; 12] 4 
        |> should equal Bet.D1_D2  

        RouletteStrategy.Strategy.strategy_2 [12;24;12;24; 24] 4 
        |> should equal Bet.D1_D2  

        RouletteStrategy.Strategy.strategy_2 [12;24;12;24; 36] 4 
        |> should equal Bet.D1_D2  

    [<Test>]
    let ``Strategy_2 <when> D3 not appeared less then the IN-A_ROW value <then> suggests None`` () =
        RouletteStrategy.Strategy.strategy_2 [12;24; 36; 12;24; ] 4 
        |> should equal Bet.None


    [<Test>]
    let ``Strategy_2 <when> D1, D2 or D3 does not appeared less then the IN-A_ROW value <then> suggests None`` () =
        RouletteStrategy.Strategy.strategy_2 [23;33;1;12;18;6;1;18;23] 4 
        |> should equal Bet.None