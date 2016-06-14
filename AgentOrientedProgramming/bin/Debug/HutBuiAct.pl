
:- dynamic dirty/2.
:- dynamic obstacle/3.
:- dynamic in/2.
:- dynamic wasweight/3.
:- dynamic choose/1.
:- dynamic leftmoves/1.
:- dynamic discover/2.
:- dynamic done/1.
:- dynamic facing/1.
:- dynamic timePassed/1.

do(suck) :- not(done(stop)), in(X,Y), dirty(X,Y).
do(stop) :- not(done(start)), leftmoves(0).
/*do(forward) :- not(done(stop)), in(X,Y), not(dirty(X,Y)), choose(M), Temp is M+360, facing(Temp).
do(turn90) :- not(done(stop)), in(X,Y), not(dirty(X,Y)), choose(M), Temp
is M+360, not(facing(Temp)).*/
do(forward) :- not(done(stop)), in(X,Y), choose(A), facing(A), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)),
not(obstacle(Temp1, Temp2, both)).
do(turn90) :- not(done(stop)), choose(M), not(facing(M)).
do(wait) :- not(done(stop)), in(X,Y), facing(A), Temp1 is X + round(cos(pi*A/180)),
Temp2 is Y + round(sin(pi*A/180)), obstacle(Temp1, Temp2, dynamic).
do(start) :- done(stop), timePassed(T), T = 21600.

choose(M) :- in(X,Y), facing(A), A1 is A+90, A2 is A+180, A3 is A+270, TempA1 is X + round(cos(pi*A/180)),
TempA2 is Y + round(sin(pi*A/180)), weight(TempA1, TempA2, WA), TempA11 is X + round(cos(pi*A1/180)), TempA12 is Y + round(sin(pi*A1/180)),
weight(TempA11, TempA12, WA1), TempA21 is X + round(cos(pi*A2/180)), TempA22 is Y + round(sin(pi*A2/180)), weight(TempA21, TempA22, WA2),
TempA31 is X + round(cos(pi*A3/180)), TempA32 is Y + round(sin(pi*A3/180)), weight(TempA31, TempA32, WA3), min([A,WA],[A1,WA1],[A2,WA2],[A3,WA3], [M,W]),
not(W=sinf).

min(L1, L2, L3, L4, M) :- min(L1, L2, M1), min(L3, L4, M2), min(M1, M2, M).
min([A1,B1],[_,B2],[A1,B1]):-(integer(B1),integer(B2),B1<B2;B2=B1;integer(B1),(B2=sinf;B2=dinf);B1=dinf,B2=sinf).
min([_,B1],[A2,B2],[A2,B2]):-(integer(B1),integer(B2),B2<B1;integer(B2),(B1=sinf;B1=dinf);B1=sinf,B2=dinf).

weight(X,Y,sinf):-obstacle(X,Y,static).
weight(X,Y,dinf):-obstacle(X,Y,dynamic).
weight(X,Y,0):-not(wasweight(X,Y,_)), not(obstacle(X,Y,both)),integer(X),integer(Y).
weight(X,Y,W):-wasweight(X,Y,W).
