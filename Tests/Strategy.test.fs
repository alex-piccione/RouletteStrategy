namespace Tests

open NUnit.Framework
open FsUnit

open RouletteStrategy.Entities

[<Category("Strategy")>]
module Strategy = 

    [<Test>]
    let ``getArea`` () =

        RouletteStrategy.Strategy.getArea 1 |> should equal Area.One
        RouletteStrategy.Strategy.getArea 12 |> should equal Area.One

        RouletteStrategy.Strategy.getArea 13 |> should equal Area.Two
        RouletteStrategy.Strategy.getArea 24 |> should equal Area.Two

        RouletteStrategy.Strategy.getArea 25 |> should equal Area.Three
        RouletteStrategy.Strategy.getArea 36 |> should equal Area.Three

    [<Test>]
    let ``Strategy_2 <when> D1 not appeared <then> suggests D2 and D3`` () =
        RouletteStrategy.Strategy.strategy_2 [24;36;24;36] 4
        |> should equal Suggestion.D2_D3

        RouletteStrategy.Strategy.strategy_2 [24;36;24;36; 12] 4
        |> should equal Suggestion.D2_D3

        RouletteStrategy.Strategy.strategy_2 [24;36;24;36; 24] 4
        |> should equal Suggestion.D2_D3

        RouletteStrategy.Strategy.strategy_2 [24;36;24;36; 36] 4
        |> should equal Suggestion.D2_D3

    [<Test>]
    let ``Strategy_2 <when> D2 not appeared <then> suggests D1 and D3`` () =
        RouletteStrategy.Strategy.strategy_2 [12;36;12;36;12] 4
        |> should equal Suggestion.D1_D3 
       
        RouletteStrategy.Strategy.strategy_2 [12;36;12;36; 12] 4
        |> should equal Suggestion.D1_D3 

        RouletteStrategy.Strategy.strategy_2 [12;36;12;36; 24] 4
        |> should equal Suggestion.D1_D3 

        RouletteStrategy.Strategy.strategy_2 [12;36;12;36; 36] 4
        |> should equal Suggestion.D1_D3 

    [<Test>]
    let ``Strategy_2 <when> D3 not appeared <then> suggests D1 and D2`` () =
        RouletteStrategy.Strategy.strategy_2 [12;24;12;24] 4 
        |> should equal Suggestion.D1_D2

        RouletteStrategy.Strategy.strategy_2 [12;24;12;24; 12] 4 
        |> should equal Suggestion.D1_D2  

        RouletteStrategy.Strategy.strategy_2 [12;24;12;24; 24] 4 
        |> should equal Suggestion.D1_D2  

        RouletteStrategy.Strategy.strategy_2 [12;24;12;24; 36] 4 
        |> should equal Suggestion.D1_D2  
