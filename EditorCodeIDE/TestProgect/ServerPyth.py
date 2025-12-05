import socket

s = socket.soket(socket.AF_INET,socket.SOCK_SREAM)
s.bind(('192.168.0.1',8888))
s.listen(2)
conn, addr = s.accept()

print('Server started')

while True:
	data = conn-recv(1024)
	if not data: break
	conn.sendall(data)
	print(data)s