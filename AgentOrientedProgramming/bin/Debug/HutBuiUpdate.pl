%assert(in(0,0)), assert(weight(0,0,1)), assert(choose(0)), assert(leftmoves(1)), assert(discover(0,0)), asser(facing(0)), do(X).

:- dynamic dirty/2.
:- dynamic obstacle/3.
:- dynamic wasin/2.
:- dynamic wasweight/3.
:- dynamic choosed/1.
:- dynamic wasleftmoves/1.
:- dynamic discovered/2.
:- dynamic done/1.
:- dynamic wasfacing/1.
:- dynamic wastimePassed/1.

%r8
in(0,0) :- done(start), assert(wasin(0,0)).
weight(0,0,0) :- done(start), assert(wasweight(0,0,0)).
choose(0) :- done(start), assert(choosed(0)).
leftmoves(0) :- done(start), assert(wasleftmoves(0)).
discover(0,0) :- done(start), assert(discovered(0,0)).
facing(0) :- done(start), assert(wasfacing(0)).
timePassed(0) :- done(start), assert(wastimePassed(0)).


facing(Temp):- wasfacing(A), Temp is A+90, done(turn90).
%r2 { in(X, Y), facing(A), weight(X, Y, W), do(forward) } → { in(X + round(cos(pi*A/180)), Y + round(sin(pi*A/180))), weight(X, Y, W + 1), choose(A) }
in(Temp1, Temp2):- wasin(X,Y), wasfacing(A), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), done(forward).
weight(X,Y,Temp):- wasin(X,Y), wasweight(X,Y,W), Temp is W+1, done(forward).
weight(X,Y,inf):-obstacle(X,Y,both).
weight(X,Y,W):-not(wasin(X,Y)), wasweight(X,Y,W).
weight(X,Y,0):-not(discovered(X,Y)), not(obstacle(X,Y,both)).
choose(M) :- wasin(X,Y), wasfacing(A), A1 is A+90, A2 is A1+90, A3 is A2+90, TempA1 is X + round(cos(pi*A/180)), TempA2 is Y + round(sin(pi*A/180)), weight(TempA1, TempA2, WA), TempA11 is X + round(cos(pi*A1/180)), TempA12 is Y + round(sin(pi*A1/180)), weight(TempA11, TempA12, WA1), TempA21 is X + round(cos(pi*A2/180)), TempA22 is Y + round(sin(pi*A2/180)), weight(TempA21, TempA22, WA2), TempA31 is X + round(cos(pi*A3/180)), TempA32 is Y + round(sin(pi*A3/180)), weight(TempA31, TempA32, WA3), min([A,WA],[A1,WA1],[A2,WA2],[A3,WA3], [M,_]), (done(forward);done(start)).
min(L1, L2, L3, L4, M) :- min(L1, L2, M1), min(L3, L4, M2), min(M1, M2, M).
min([_,B1],[A2,B2],[A2,B2]):-integer(B1),integer(B2),(B2<B1;B2=B1;B1=inf).
min([A1,B1],[_,B2],[A1,B1]):-integer(B1),integer(B2),(B1<B2;B2=inf).
/*choose(A):- wasfacing(A), done(forward).
choose(A):- wasin(X,Y), wasfacing(A), choosed(M), Temp1 is X + round(cos(pi*M/180)), Temp2 is Y + round(sin(pi*M/180)), obstacle(Temp1, Temp2, both), done(turn90).
choose(A):- wasfacing(A), choosed(M), wasin(X, Y), Temp1 is X + round(cos(pi*M/180)), Temp2 is Y + round(sin(pi*M/180)), not(obstacle(Temp1, Temp2,both)), Temp3 is X + round(cos(pi*A/180)), Temp4 is Y + round(sin(pi*A/180)), not(obstacle(Temp3, Temp4, both)), wasweight(Temp3, Temp4, W1), wasweight(Temp1, Temp2, W2), W1 < W2,  done(turn90).
%%r5 { facing(a), in(X, Y), not(obstacle(x + round(cos(pi*A/180)), y + round(sin(pi*A/180)), static)), not(discover(x + round(cos(pi*A/180)), y + round(sin(pi*A/180)))), leftmoves(k) } → { discover(x + round(cos(pi*A/180)), y + round(sin(pi*A/180))), leftmoves(k + 1) }*/

discover(Temp1, Temp2):- wasfacing(M), (A is M;A is M+90;A is M+180;A is M+270), wasin(X,Y), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), not(obstacle(Temp1, Temp2, static)), not(discovered(Temp1, Temp2)), (done(forward);done(start)).
leftmoves(TempK):- wasfacing(M), (A is M;A is M+90;A is M+180;A is M+270), wasin(X,Y), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), not(obstacle(Temp1, Temp2, static)), not(discovered(Temp1, Temp2)), wasleftmoves(K), TempK is K + 1, retract(wasleftmoves(K)), assert(wasleftmoves(TempK)), (done(forward);done(start)).
/*discover(Temp1, Temp2):- not(done(stop)), wasfacing(A), wasin(X,Y), Temp1 is X + round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)), not(obstacle(Temp1, Temp2, static)), not(discovered(Temp1, Temp2)).
leftmoves(TempK):- not(done(stop)), wasfacing(A), wasin(X,Y), Temp1 is X
+ round(cos(pi*A/180)), Temp2 is Y + round(sin(pi*A/180)),
not(obstacle(Temp1, Temp2, static)), not(discovered(Temp1, Temp2)),
wasleftmoves(K), TempK is K + 1.*/
leftmoves(TempK):- wasin(X,Y), wasweight(X,Y,W), W > 0, wasleftmoves(K), TempK is K - 1, done(forward).
timePassed(Temp):- wastimePassed(T), Temp is T+1, done(stop).