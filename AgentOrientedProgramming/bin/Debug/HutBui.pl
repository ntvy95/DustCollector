do(suck):- not(do(stop)), in(X,Y), dirty(X,Y).
do(stop):- not(do(stop)), in(X,Y), not(dirty(X,Y)), leftmoves(0).
do(wait):- not(do(stop)), in(X,Y), not(dirty(X,Y)), leftmoves(1), facing(A), obstacle(X + cos(A), Y + sin(A), dynamic).
do(forward):- not(do(stop)), in(X, Y), not(dirty(X,Y)), leftmoves(K), K > 0, choose(M), facing(M + 360).
do(turn90):- not(do(stop)), in(X,Y), not(dirty(X,Y)), leftmoves(K), K > 0, choose(M), not(facing(M + 360)).
do(start):- time(T), T =:= 21600.


facing(A+90):- facing(A), do(turn90).
%r2 { in(X, Y), facing(A), weight(X, Y, W), do(forward) } → { in(X + cos(A), Y + sin(A)), weight(X, Y, W + 1), choose(A) }
in(X + cos(A), Y + sin(A)):- in(X,Y), facing(A), do(forward).
weight(X,Y,W + 1):- in(X,Y), weight(X,Y,W), do(forward).
choose(A):- facing(A), do(forward).

choose(A):- in(X,Y), facing(A), choose(M), obstacle(X + cos(M), Y + sin(M), both), do(turn90).
choose(A):- facing(A), in(X, Y), not(obstacle(X + cos(M), Y + sin(M),both)), not(obstacle(X + cos(A), Y + sin(A), both)), choose(M), weight(X + cos(A), Y + sin(A), W1), weight(X + cos(M), Y + sin(M), W2), W1 < W2,  do(turn90).
%r5 { facing(a), in(X, Y), not(obstacle(x + cos(a), y + sin(a), static)), not(discover(x + cos(a), y + sin(a))), leftmoves(k) } → { discover(x + cos(a), y + sin(a)), leftmoves(k + 1) }

discover(X + cos(A), Y + sin(A)):- not(do(stop)), facing(A), in(X,Y), not(obstacle(X + cos(A), Y + sin(A), static)), not(discover(X + cos(A), Y + sin(A))).
leftmoves(K+1):- not(do(stop)), facing(A), in(X,Y), not(obstacle(X + cos(A), Y + sin(A), static)), not(discover(X + cos(A), Y + sin(A))), leftmoves(K).

leftmoves(K-1):- in(X,Y), weight(X,Y,1), leftmoves(K), do(forward).
time(T+1):- time(T), do(stop).

%r8
in(0,0):- do(start).
weight(0,0,1):- do(start).
choose(0):- do(start).
leftmoves(1):- do(start).
discover(0,0):- do(start).
facing(0) :- do(start).