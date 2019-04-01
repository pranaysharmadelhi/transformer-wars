# Transformer Wars
## Autobots vs Decepticon

### Swagger:
1) Go to following URL in browser 
```
https://transformer-wars.herokuapp.com/
```

### API:

1) Make following GET call to the REST API
```
https://transformer-wars.herokuapp.com/api/Wars/
```


### Rules:
1) If Optimus is on one team and Predaking is on another team, the war ends **with no victors on either side. **
2) If a Transformer is named Optimus or Predaking, the battle ends automatically with
them as the victor
3) If Transformer A exceeds Transformer B in strength by 3 or more and Transformer B
has less than 5 courage, the battle is won by Transformer A (Transformer B ran away)
4) If Transformer A’s skill rating exceeds Transformer B's rating by 5 or more, Transformer
A wins the fight.
5) Otherwise, the victor is whomever of Transformer A and B has the higher overall rating.
(In the event of a tie between two robots, lucky (random) one wins).

