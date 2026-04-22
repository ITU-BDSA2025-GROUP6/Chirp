## Security Incident Report — Monitoring Droplet:

Date discovered: 2026-04-21

What happened:
- The preprod-deploy SSH key was compromised
- Attacker gained root access using key fingerprint iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
- Accessed from multiple Azure IPs: 52.161.50.37, 172.214.45.246, 172.184.174.144, 172.215.239.51
- Deployed a cryptocurrency miner disguised as /tmp/mysql
- Created a fake dnsmasq user as persistence mechanism
- Ongoing brute force scanning started 2026-04-19 from multiple IPs

How it was cleaned: Teammate killed miner process, locked dnsmasq user, removed rogue containers

Remediation: Destroy and rebuild droplet with hardened SSH config


## full attacker IP list and more for rapport guys:
```
root@minitwit-app:/opt/monitoring# cat /root/.bash_history
apt update
apt install -y docker.io docker-compose-v2 || apt install -y docker.io docker-compose-plugin
systemctl enable docker
systemctl start docker
docker --version
docker compose version
exit
cd minitwit/
docker compose up -d --build
docker compose ps
sudo tar -czf /opt/itu-minitwit/db_backup_$(date +%F-%H%M).tar.gz -C /opt/itu-minitwit data
ls -lh /opt/itu-minitwit/db_backup_*.tar.gz | tail -n 1
cd /opt
cd digitalocean/
ls
cd ..
ls
cd root/
ls
cd minitwit/
ls
exit
ls
cd minitwit/
ls
cd ..
rm -rf minitwit/
ls
# On the droplet:
mkdir -p ~/.ssh
echo "PASTE_YOUR_PUBLIC_KEY_HERE" >> ~/.ssh/authorized_keys
chmod 600 ~/.ssh/authorized_keys
ls
cd .ssh
ls
nano authorized_keys
..
cd .
cd ..
#!/usr/bin/env bash
set -euo pipefail
# ── 1. Install Docker ──
curl -fsSL https://get.docker.com | sh
cd /opt
cd /root
ls
nano .setup-preprod-droplet.sh
chmod +x .setup-preprod-droplet.sh
./.setup-preprod-droplet.sh
docker login
mkdir -p /opt/itu-minitwit/data
cd /opt/itu-minitwit/
ls
ssh-keygen -t ed25519 -C "preprod-deploy" -f ~/.ssh/minitwit_preprod
cat ~/.ssh/minitwit_preprod
exit
docker pull sebseb10/itu-minitwit:latest
curl -fsSL https://get.docker.com | sh
systemctl enable docker
systemctl start docker
docker --version
docker compose version
cd /opt
# Remove the empty directory we just made
rm -rf itu-minitwit
# Clone the repo
git clone https://github.com/sebseb10/ITU-MiniTwit.git itu-minitwit
cd itu-minitwit
export DOCKER_USERNAME=sebseb10
export TAG="latest"
chmod +x scripts/deploy.sh
bash scripts/deploy.sh
systemctl start docker
systemctl enable docker
docker ps
cd /opt/itu-minitwit
TAG=latest DOCKER_USERNAME=sebseb10 bash scripts/deploy.sh
exit
cd .ssh
ls
exit
cd .ssh
ls
ls -la ~/.ssh
cat ~/.ssh/authorized_keys
nano authorized_keys
nano minitwit_preprod
cat /root/.ssh/minitwit_preprod.pub
nano authorized_keys
cat /root/.ssh/minitwit_preprod.pub >> /root/.ssh/authorized_keys
chmod 600 /root/.ssh/authorized_keys
chmod 700 /root/.ssh
grep -F "$(cat /root/.ssh/minitwit_preprod.pub)" /root/.ssh/authorized_keys
cat /root/.ssh/minitwit_preprod
clear
docker ps --format "table {{.Names}}\t{{.Ports}}\t{{.Status}}"
sudo ss -ltnp | grep 5001
docker stop minitwit-chirp-web-1
docker rm minitwit-chirp-web-1
cd /opt/itu-minitwit
docker compose ps
docker ps --format "table {{.Names}}\t{{.Image}}\t{{.Ports}}\t{{.Status}}"
docker compose logs --tail=200 itu-minitwit
curl -i http://localhost:5001/
ss -ltnp | grep 5001
cat /opt/itu-minitwit/.env
docker exec -it itu-minitwit-itu-minitwit-1 printenv | grep ConnectionStrings
docker compose logs --tail=50 itu-minitwit
grep '^ConnectionStrings__DefaultConnection=' /opt/itu-minitwit/.env | cat -A
clear
cat /opt/itu-minitwit/docker-compose.yml
docker compose up
docker compose down
docker compose up -build
docker compose up
docker compose ps
docker ps -a --format "table {{.Names}}\t{{.Image}}\t{{.Status}}"
docker compose logs --tail=100 itu-minitwit
cd /opt/itu-minitwit/
ls
docker compose ps
docker compose logs --tail=100 itu-minitwit
clear
cd /opt/itu-minitwit
docker compose ps
docker compose logs --tail=100 itu-minitwit
docker compose port itu-minitwit 5001
curl -I http://localhost:5001
clear
ss -tulpn | grep 5001
grep '^ConnectionStrings__DefaultConnection=' /opt/itu-minitwit/.env
nc -vz 207.154.201.230 25060
nc -vz 207.154.201.230 25060
apt update && apt install -y netcat-openbsd
nc -vz 207.154.201.230 25060
docker compose --profile localdb up -d
grep '^ConnectionStrings__DefaultConnection=' /opt/itu-minitwit/.env
nc -vz 207.154.201.230 25060
cd /opt/itu-minitwit
docker compose restart itu-minitwit
docker compose logs --tail=100 itu-minitwit
nc -vz 207.154.201.230 25060
cat > /opt/itu-minitwit/.env <<'EOF'
DOCKER_USERNAME=sebseb10
TAG=16467b9f14c58d9bf67b00cd94ce14180e720c33
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5001
ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=chirp;Username=postgres;Password=postgres
EOF

cd /opt/itu-minitwit
docker compose --profile localdb up -d
docker compose logs --tail=100 itu-minitwit
grep '^ConnectionStrings__DefaultConnection=' /opt/itu-minitwit/.env
ls
rm -rm minitwit/
rm -rf minitwit/
ls
cd /opt
ls
rm -rf itu-minitwit/
ls
mkdir monitoring
docker --version
curl -fsSL https://get.docker.com | sh
curl -fsSL https://get.docker.com | sh
cd .ssh
ls
cat minitwit_preprod
ls -la /opt/monitoring/
cd /opt/monitoring/monitoring/
cat docker-compose.monitoring.yml
docker rm -f grafana
docker rm -f prometheus
cat /opt/monitoring/monitoring/prometheus/prometheus.yml
clear
cd /opt/monitoring/monitoring
docker compose -f docker-compose.monitoring.yml restart grafana
docker logs alloy --tail 50
docker ps
docker compose -f docker-compose.monitoring.yml restart grafana
curl -k https://itu-minitwit.duckdns.org/metrics | head -
curl -k https://itu-minitwit.duckdns.org/metrics | head -5
dig +short itu-minitwit.duckdns.org
exit
clear
dig +short itu-minitwit.duckdns.org
curl -k https://46.101.231.189/metrics -H "Host: itu-minitwit.duckdns.org" 2>&1 | head -5
tail -20 /var/log/nginx/access.log
curl -k https://itu-minitwit.duckdns.org/metrics | head -5
curl http://localhost:9090/api/v1/targets | python3 -m json.tool | head -30
cat /prometheus.yml
cd /opt/monitoring/monitoring/prometheus/
cat prometheus.yml
cd /opt/monitoring/monitoring
docker compose -f docker-compose.monitoring.yml restart prometheus
cd /nginx
cd ~/nginx
cd ..
ls
cd opt
ls
exit
docker ps
htop

kill -9 3772635
ls
cd .ssh/
ls
cat authorized_keys
cd
kill -9 3772635
file /tmp/mysql
rkhunter --check
# Recent auth attempts
journalctl -u ssh | grep "Accepted\|Failed" | tail -50
# Grafana query load — look for long-running or repeated queries
journalctl -u grafana-server | grep -i "query\|slow\|timeout" | tail -50
ps aux --sort=-%cpu | head -20
pkill -9 -u dnsmasq
usermod -L dnsmasq
ps aux --sort=-%cpu | head -20
ls
docker ps
ss -tulpn | grep 5432
# Most important — what network connections does it have?
ss -tulpn | grep $(pgrep -f /tmp/mysql)
# or
cat /proc/$(pgrep -f /tmp/mysql)/net/tcp
# Where is it connecting to?
nsenter -t $(pgrep -f /tmp/mysql) -n ss -tnp
# Check DNS lookups it's making
tcpdump -i any -n port 53 -l | grep -i mysql &
sleep 10 && kill %1docker ps
cd /opt/monitoring/
ls
cd monitoring/
ls
cat docker-compose.monitoring.yml
# Check when it was created and by whom
docker inspect chirp-postgres | grep -E "Created|StartedAt"
# Check what's inside it
docker exec chirp-postgres psql -U postgres -c "\l"   # list databases
docker exec chirp-postgres psql -U postgres -c "\du"  # list users
# Check its logs for connections
docker logs chirp-postgres 2>&1 | tail -50
# 1. Stop and remove the rogue postgres
docker stop chirp-postgres
docker rm chirp-postgres
# 2. Check your docker-compose.yml - postgres shouldn't be in it
cat docker-compose.yml | grep -A 20 postgres
# 3. Also check if the app droplet's postgres also has priv_esc user!
# SSH into your OTHER droplet and run:
docker exec <postgres-container> psql -U postgres -c "\du"
docker ps
docker kill 57a4ff44b40b
docker ps
exit
docker inspect grafana | grep -i network
docker inspect alloy | grep -A 20 "Networks"
docker logs alloy --tail 50
ls ~/
docker ps
find / -name "docker-compose*.yml" 2>/dev/null
find /opt/monitoring -name "*.alloy" 2>/dev/null
cat /opt/monitoring/monitoring/alloy/config.alloy
cat /opt/monitoring/docker-compose.yml
cat /opt/monitoring/monitoring/docker-compose.monitoring.yml
nano /opt/monitoring/docker-compose.yml
cd /opt/monitoring
docker compose up -d alloy
docker ps | grep alloy
docker inspect alloy | grep -A 30 "Networks"
ls
cat /opt/monitoring/docker-compose.yml | grep -A 20 "alloy"
cat /opt/monitoring/docker-compose.yml | grep -A 10 "^networks"
cat /opt/monitoring/docker-compose.yml | grep -A 20 "alloy"
cat /opt/monitoring/docker-compose.yml | grep -A 10 "^networks"
docker network ls
cd /opt/monitoring
docker compose up -d --force-recreate alloy
docker inspect alloy | grep -A 5 "monitoring_monitoring-network"
docker compose restart alloy
cat /opt/monitoring/monitoring/loki/loki-config.yml | grep -i "reject\|retention\|max_stream"
docker compose restart alloy
docker inspect alloy | grep -A 5 "monitoring_monitoring-network"
ps aux | grep /tmp/mysql
# Check if the bash watchdogs are still running
ps aux | grep dnsmasq
# Find the persistence NOW before it respawns
crontab -u dnsmasq -l 2>/dev/null
cat /var/spool/cron/crontabs/dnsmasq 2>/dev/null
ls -la /home/dnsmasq/.config/systemd/user/ 2>/dev/null
cat /home/dnsmasq/.bashrc 2>/dev/null
cat /home/dnsmasq/.profile 2>/dev/null
# Check /tmp for the dropper script
ls -la /tmp/
cat /opt/monitoring/monitoring/docker-compose.yml
cat /opt/monitoring/monitoring/docker-compose.monitoring.yml
docker stop chirp-postgres itu-minitwit-itu-minitwit-1
docker rm chirp-postgres itu-minitwit-itu-minitwit-1
find / -name "docker-compose*" 2>/dev/null
cat /opt/monitoring/docker-compose.yml
root@minitwit-app:/opt/monitoring# grep "dnsmasq\|mysql\|/tmp" /root/.bash_history | head -30
file /tmp/mysql
pkill -9 -u dnsmasq
usermod -L dnsmasq
ss -tulpn | grep $(pgrep -f /tmp/mysql)
cat /proc/$(pgrep -f /tmp/mysql)/net/tcp
nsenter -t $(pgrep -f /tmp/mysql) -n ss -tnp
tcpdump -i any -n port 53 -l | grep -i mysql &
ps aux | grep /tmp/mysql
ps aux | grep dnsmasq
crontab -u dnsmasq -l 2>/dev/null
cat /var/spool/cron/crontabs/dnsmasq 2>/dev/null
ls -la /home/dnsmasq/.config/systemd/user/ 2>/dev/null
cat /home/dnsmasq/.bashrc 2>/dev/null
cat /home/dnsmasq/.profile 2>/dev/null
# Check /tmp for the dropper script
ls -la /tmp/
root@minitwit-app:/opt/monitoring# last -F | grep -v "130.226\|37.97\|77.33" | head -20
root     pts/0        162.243.188.66   Sat Mar 21 09:51:59 2026 - Sat Mar 21 11:05:10 2026  (01:13)
reboot   system boot  6.8.0-71-generic Mon Feb 23 18:22:16 2026   still running

wtmp begins Mon Feb 23 18:22:16 2026
```

## The Attack
This is s a constant brute force scan starting April 19th from multiple IPs. The server was being hammered with invalid user attempts for days before the actual compromise:

```
root@minitwit-app:/opt/monitoring# grep "Accepted\|Failed\|Invalid" /var/log/auth.log | grep -v "130.226\|37.97\|smaja\|anys\|toon\|pann" | head -50
2026-04-19T00:01:32.933043+00:00 minitwit-app sshd[1727812]: Invalid user sol from 80.94.92.184 port 34070
2026-04-19T00:04:23.789257+00:00 minitwit-app sshd[1742266]: Invalid user solana from 80.94.92.184 port 36700
2026-04-19T00:07:15.280556+00:00 minitwit-app sshd[1756221]: Invalid user solana from 80.94.92.184 port 39310
2026-04-19T00:10:04.579328+00:00 minitwit-app sshd[1769978]: Invalid user ubuntu from 80.94.92.184 port 41948
2026-04-19T00:12:53.975292+00:00 minitwit-app sshd[1783278]: Invalid user ubuntu from 80.94.92.184 port 44560
2026-04-19T00:15:40.635169+00:00 minitwit-app sshd[1796278]: Invalid user ubuntu from 80.94.92.184 port 47188
2026-04-19T00:18:26.978200+00:00 minitwit-app sshd[1808671]: Invalid user ubuntu from 80.94.92.184 port 49812
2026-04-19T00:23:57.041360+00:00 minitwit-app sshd[1834378]: Invalid user admin from 45.148.10.121 port 48008
2026-04-19T00:30:20.764804+00:00 minitwit-app sshd[1863951]: Invalid user user from 2.57.121.25 port 33596
2026-04-19T00:43:43.917045+00:00 minitwit-app sshd[1924962]: Invalid user admin from 200.126.105.149 port 7626
2026-04-19T00:56:16.872310+00:00 minitwit-app sshd[1980748]: Invalid user kimberlee from 213.209.159.159 port 30289
2026-04-19T01:03:41.150801+00:00 minitwit-app sshd[2011579]: Invalid user admin from 2.57.121.112 port 44258
2026-04-19T01:09:49.234617+00:00 minitwit-app sshd[2037363]: Invalid user AdminGPON from 45.148.10.121 port 45550
2026-04-19T01:12:45.637226+00:00 minitwit-app sshd[2049678]: Invalid user administrator from 142.248.80.104 port 58062
2026-04-19T01:37:52.512520+00:00 minitwit-app sshd[2159880]: Invalid user kt from 61.151.249.194 port 39804
2026-04-19T01:38:04.469285+00:00 minitwit-app sshd[2160916]: Invalid user n8n from 135.235.138.43 port 34688
2026-04-19T01:41:41.759380+00:00 minitwit-app sshd[2176332]: Invalid user ubuntu from 135.235.138.43 port 57054
2026-04-19T01:44:59.164876+00:00 minitwit-app sshd[2190866]: Invalid user soporte from 135.235.138.43 port 37366
2026-04-19T01:47:34.501629+00:00 minitwit-app sshd[2203362]: Invalid user user from 2.57.121.25 port 13137
2026-04-19T01:50:11.529892+00:00 minitwit-app sshd[2214847]: Invalid user sammy from 135.235.138.43 port 57174
2026-04-19T01:51:11.430213+00:00 minitwit-app sshd[2219209]: Invalid user 1234 from 193.46.255.86 port 19902
2026-04-19T01:51:53.059610+00:00 minitwit-app sshd[2222142]: Invalid user ftptest from 135.235.138.43 port 39992
2026-04-19T01:53:32.704894+00:00 minitwit-app sshd[2229633]: Invalid user ftpuser from 135.235.138.43 port 58690
2026-04-19T01:55:14.047810+00:00 minitwit-app sshd[2237419]: Invalid user tim from 135.235.138.43 port 43796
2026-04-19T01:56:11.459395+00:00 minitwit-app sshd[2241983]: Invalid user git from 61.151.249.194 port 57842
2026-04-19T01:56:55.164487+00:00 minitwit-app sshd[2245592]: Invalid user postgres from 135.235.138.43 port 56664
2026-04-19T01:57:20.459549+00:00 minitwit-app sshd[2247540]: Invalid user test8 from 61.151.249.194 port 46672
2026-04-19T01:58:34.347630+00:00 minitwit-app sshd[2253240]: Invalid user postgres from 135.235.138.43 port 35462
2026-04-19T02:00:17.011335+00:00 minitwit-app sshd[2261153]: Invalid user n8n from 135.235.138.43 port 38158
2026-04-19T02:03:48.969091+00:00 minitwit-app sshd[2277333]: Invalid user ftptest from 135.235.138.43 port 54980
2026-04-19T02:05:32.561841+00:00 minitwit-app sshd[2285104]: Invalid user steam from 135.235.138.43 port 41842
2026-04-19T02:09:00.956249+00:00 minitwit-app sshd[2301835]: Invalid user postgres from 135.235.138.43 port 60754
2026-04-19T02:09:45.901160+00:00 minitwit-app sshd[2305442]: Invalid user mackenna from 213.209.159.159 port 8373
2026-04-19T02:12:19.687303+00:00 minitwit-app sshd[2317318]: Invalid user mango from 135.235.138.43 port 39748
2026-04-19T02:14:03.840858+00:00 minitwit-app sshd[2325456]: Invalid user ftpuser from 135.235.138.43 port 47978
2026-04-19T02:17:47.442437+00:00 minitwit-app sshd[2342798]: Invalid user david from 61.151.249.194 port 43820
2026-04-19T02:18:11.725300+00:00 minitwit-app sshd[2344762]: Invalid user admin from 2.57.121.112 port 8795
2026-04-19T02:18:51.828987+00:00 minitwit-app sshd[2347855]: Invalid user pacs from 61.151.249.194 port 60910
2026-04-19T02:18:58.535754+00:00 minitwit-app sshd[2348460]: Invalid user user from 45.148.10.121 port 44506
2026-04-19T02:21:00.594056+00:00 minitwit-app sshd[2358002]: Invalid user ftptest from 135.235.138.43 port 41902
2026-04-19T02:44:16.204693+00:00 minitwit-app sshd[2468020]: Invalid user administrator from 142.248.80.104 port 41556
2026-04-19T03:04:33.739134+00:00 minitwit-app sshd[2564012]: Invalid user user from 2.57.121.25 port 20326
2026-04-19T03:22:40.583379+00:00 minitwit-app sshd[2640724]: Invalid user mallorie from 213.209.159.159 port 56078
2026-04-19T03:25:22.417264+00:00 minitwit-app sshd[2649137]: Invalid user ubnt from 45.148.10.121 port 48262
2026-04-19T03:32:49.085404+00:00 minitwit-app sshd[2682036]: Invalid user admin from 2.57.121.112 port 54739
2026-04-19T04:17:16.305310+00:00 minitwit-app sshd[2886981]: Invalid user administrator from 142.248.80.104 port 56760
2026-04-19T04:20:35.675501+00:00 minitwit-app sshd[2900918]: Invalid user a from 154.144.243.138 port 54534
2026-04-19T04:21:30.311348+00:00 minitwit-app sshd[2905044]: Invalid user user from 2.57.121.25 port 9845
2026-04-19T04:23:15.086822+00:00 minitwit-app sshd[2912323]: Invalid user  from 47.88.30.94 port 42950
2026-04-19T04:25:27.118795+00:00 minitwit-app sshd[2921510]: Invalid user test from 45.148.10.121 port 42128
```


## FUll terminal logs leading to the discorvery of the hacker!

```
root@minitwit-app:/opt/monitoring#   top -bn1 | head -20
ps aux --sort=-%cpu | head -20
who
last | head -20
top - 07:58:51 up 57 days, 13:36,  2 users,  load average: 0.11, 0.03, 0.01
Tasks: 119 total,   1 running, 118 sleeping,   0 stopped,   0 zombie
%Cpu(s): 10.0 us,  0.0 sy,  0.0 ni, 90.0 id,  0.0 wa,  0.0 hi,  0.0 si,  0.0 st
MiB Mem :    961.6 total,    100.5 free,    669.0 used,    347.4 buff/cache     
MiB Swap:      0.0 total,      0.0 free,      0.0 used.    292.6 avail Mem

    PID USER      PR  NI    VIRT    RES    SHR S  %CPU  %MEM     TIME+ COMMAND
      1 root      20   0   22792  10328   6016 S   0.0   1.0  53:56.12 systemd
      2 root      20   0       0      0      0 S   0.0   0.0   0:01.13 kthreadd
      3 root      20   0       0      0      0 S   0.0   0.0   0:00.00 pool_wo+
      4 root       0 -20       0      0      0 I   0.0   0.0   0:00.00 kworker+
      5 root       0 -20       0      0      0 I   0.0   0.0   0:00.00 kworker+
      6 root       0 -20       0      0      0 I   0.0   0.0   0:00.00 kworker+
      7 root       0 -20       0      0      0 I   0.0   0.0   0:00.00 kworker+
     12 root       0 -20       0      0      0 I   0.0   0.0   0:00.00 kworker+
     13 root      20   0       0      0      0 I   0.0   0.0   0:00.00 rcu_tas+
     14 root      20   0       0      0      0 I   0.0   0.0   0:00.00 rcu_tas+
     15 root      20   0       0      0      0 I   0.0   0.0   0:00.00 rcu_tas+
     16 root      20   0       0      0      0 S   0.0   0.0  36:41.41 ksoftir+
     17 root      20   0       0      0      0 I   0.0   0.0  89:56.44 rcu_pre+
USER         PID %CPU %MEM    VSZ   RSS TTY      STAT START   TIME COMMAND
root          44  1.2  0.0      0     0 ?        S    Feb23 1052:12 [kswapd0]
10001    1425307  1.0 10.3 1615836 101636 ?      Ssl  Apr02 293:03 /usr/bin/loki -config.file=/etc/loki/local-config.yaml
472      2687716  0.7 12.3 2057336 121548 ?      Ssl  Mar21 333:12 grafana server --homepath=/usr/share/grafana --config=/etc/grafana/grafana.ini --packaging=docker cfg:default.log.mode=console cfg:default.paths.data=/var/lib/grafana cfg:default.paths.logs=/var/log/grafana cfg:default.paths.plugins=/var/lib/grafana/plugins cfg:default.paths.provisioning=/etc/grafana/provisioning
root      290933  0.3  4.4 1995080 43680 ?       Ssl  Mar10 229:51 /usr/bin/dockerd -H fd:// --containerd=/run/containerd/containerd.sock
nobody   1843984  0.3  5.6 2299232 55668 ?       Ssl  Mar28 110:06 /bin/prometheus --config.file=/etc/prometheus/prometheus.yml
root     3932761  0.2  2.0 373088 20608 ?        Ssl  07:58   0:00 /usr/libexec/packagekitd
root     3932545  0.1  1.0  15000 10624 ?        Ss   07:54   0:00 sshd: root@pts/1
root     3932512  0.1  0.0      0     0 ?        I    07:40   0:01 [kworker/0:1-events]
root          17  0.1  0.0      0     0 ?        I    Feb23  89:56 [rcu_preempt]
root     3387502  0.0  2.0 1720172 20540 ?       Ssl  Apr17   6:43 /usr/bin/containerd
root      286833  0.0  0.4 1240956 4388 ?        Ssl  Mar10  48:51 /opt/digitalocean/bin/droplet-agent
root           1  0.0  1.0  22792 10328 ?        Ss   Feb23  53:56 /usr/lib/systemd/systemd --system --deserialize=78
root     3932810  0.0  0.0      0     0 ?        I    07:58   0:00 [kworker/u2:1-events_power_efficient]
root     2687690  0.0  0.5 1235092 5120 ?        Sl   Mar21  22:34 /usr/bin/containerd-shim-runc-v2 -namespace moby -id 4c6ed725a5bdec0772d7a5caa9a3f108226f03e4d609bc3e2736486f0df5aa1c -address /run/containerd/containerd.sock
root          16  0.0  0.0      0     0 ?        S    Feb23  36:41 [ksoftirqd/0]
root     1425258  0.0  0.5 1235348 5120 ?        Sl   Apr02  10:08 /usr/bin/containerd-shim-runc-v2 -namespace moby -id 1104a9c2113a0ceb5dd7abff1cddd086c63976668f25907072488fdb5b57d801 -address /run/containerd/containerd.sock
root     3768662  0.0  1.0 648944 10536 ?        Ssl  Apr15   3:02 /usr/libexec/fwupd/fwupd
root     1843942  0.0  0.5 1235348 5112 ?        Sl   Mar28  10:40 /usr/bin/containerd-shim-runc-v2 -namespace moby -id c72d0985ee841850d9a023780946029d8f477594ac2c61561d7c04d252ed102d -address /run/containerd/containerd.sock
root     3621632  0.0  1.7 156852 17408 ?        S<s  Apr09   5:04 /usr/lib/systemd/systemd-journald
root     pts/1        2026-04-22 07:54 (77.33.156.74)
root     pts/1        77.33.156.74     Wed Apr 22 07:54   still logged in
root     pts/2        130.226.132.96   Tue Apr 21 09:27 - 13:43  (04:16)
root     pts/1        130.226.132.96   Tue Apr 21 09:22 - 09:41  (00:18)
root     pts/0        130.226.132.96   Tue Apr 21 08:47 - 09:12  (00:24)
root     pts/0        130.226.132.96   Tue Apr 14 06:26 - 07:27  (01:00)
root     pts/0        37.97.2.70       Fri Apr 10 12:58 - 12:59  (00:00)
root     pts/0        37.97.2.185      Sat Mar 28 12:21 - 14:52  (02:30)
root     pts/0        37.97.2.185      Sat Mar 28 12:18 - 12:21  (00:03)
root     pts/0        130.226.132.96   Tue Mar 24 07:24 - 08:00  (00:35)
root     pts/0        37.97.2.67       Sat Mar 21 12:20 - 14:39  (02:19)
root     pts/1        37.97.2.67       Sat Mar 21 11:05 - 13:42  (02:37)
root     pts/0        162.243.188.66   Sat Mar 21 09:51 - 11:05  (01:13)
root     pts/1        130.226.132.96   Wed Mar 11 09:20 - 11:48  (02:27)
root     pts/0        130.226.132.96   Wed Mar 11 09:14 - 09:32  (00:17)
root     pts/0        37.97.2.178      Tue Mar 10 19:50 - 07:43  (11:53)
root     pts/0        37.97.2.178      Tue Mar 10 19:45 - 19:48  (00:02)
root     pts/0        37.97.2.178      Tue Mar 10 18:54 - 19:21  (00:26)
root     pts/0        37.97.2.178      Tue Mar 10 18:42 - 18:53  (00:10)
root     pts/1        37.97.2.178      Tue Mar 10 18:40 - 18:40  (00:00)
root     pts/0        37.97.2.178      Tue Mar 10 18:32 - 18:42  (00:09)
root@minitwit-app:/opt/monitoring#   w
netstat -tulpn
crontab -l
ls -la /root/.ssh/authorized_keys
cat /root/.ssh/authorized_keys
find /tmp /var/tmp -type f 2>/dev/null
07:58:59 up 57 days, 13:36,  2 users,  load average: 0.10, 0.03, 0.01
USER     TTY      FROM             LOGIN@   IDLE   JCPU   PCPU  WHAT
root              77.33.156.74     07:54   22:33m  0.00s  0.35s sshd: root@pts/1
Active Internet connections (only servers)
Proto Recv-Q Send-Q Local Address           Foreign Address         State       PID/Program name    
tcp        0      0 127.0.0.54:53           0.0.0.0:*               LISTEN      3621612/systemd-res
tcp        0      0 0.0.0.0:9090            0.0.0.0:*               LISTEN      1844070/docker-prox
tcp        0      0 0.0.0.0:3000            0.0.0.0:*               LISTEN      2687747/docker-prox
tcp        0      0 127.0.0.53:53           0.0.0.0:*               LISTEN      3621612/systemd-res
tcp        0      0 0.0.0.0:3100            0.0.0.0:*               LISTEN      1425364/docker-prox
tcp        0      0 0.0.0.0:22              0.0.0.0:*               LISTEN      1/systemd           
tcp6       0      0 :::9090                 :::*                    LISTEN      1844075/docker-prox
tcp6       0      0 :::3000                 :::*                    LISTEN      2687752/docker-prox
tcp6       0      0 :::3100                 :::*                    LISTEN      1425371/docker-prox
tcp6       0      0 :::22                   :::*                    LISTEN      1/systemd           
udp        0      0 127.0.0.54:53           0.0.0.0:*                           3621612/systemd-res
udp        0      0 127.0.0.53:53           0.0.0.0:*                           3621612/systemd-res
no crontab for root
-rw------- 1 root root 658 Mar 21 09:56 /root/.ssh/authorized_keys
ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIN26myk+043MivmI5zdReQQw7uukpuicKxw1sa38DOyc preprod-deploy
ssh-ed25519 <.pub key> anys@itu.dk
ssh-ed25519 <.pub key> smaja@pop-os
ssh-ed25519 <.pub key> toon@itu.dk
ssh-ed25519 <.pub key> pann@itu.dk
ssh-ed25519 <.pub key> anys@itu.dk
ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIN26myk+043MivmI5zdReQQw7uukpuicKxw1sa38DOyc preprod-deploy
root@minitwit-app:/opt/monitoring# grep "Accepted" /var/log/auth.log | tail -30
2026-04-21T09:37:11.784716+00:00 minitwit-app sshd[3925975]: Accepted publickey for root from 52.161.50.37 port 44034 ssh2: ED25519 SHA256:iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
2026-04-21T10:22:57.777887+00:00 minitwit-app sshd[3926661]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T11:22:58.034357+00:00 minitwit-app sshd[3926911]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T12:22:58.537359+00:00 minitwit-app sshd[3927127]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T13:22:58.784287+00:00 minitwit-app sshd[3927291]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T14:22:59.024229+00:00 minitwit-app sshd[3927541]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T16:22:12.012707+00:00 minitwit-app sshd[3927873]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T17:21:42.920422+00:00 minitwit-app sshd[3928106]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T18:21:53.712303+00:00 minitwit-app sshd[3928339]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T19:14:57.472460+00:00 minitwit-app sshd[3928537]: Accepted publickey for root from 172.214.45.246 port 39984 ssh2: ED25519 SHA256:iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
2026-04-21T19:14:59.741040+00:00 minitwit-app sshd[3928633]: Accepted publickey for root from 172.214.45.246 port 39985 ssh2: ED25519 SHA256:iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
2026-04-21T19:15:01.892598+00:00 minitwit-app sshd[3928680]: Accepted publickey for root from 172.214.45.246 port 39986 ssh2: ED25519 SHA256:iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
2026-04-21T19:21:54.256458+00:00 minitwit-app sshd[3928862]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T19:33:47.179100+00:00 minitwit-app sshd[3928897]: Accepted publickey for root from 172.184.174.144 port 60416 ssh2: ED25519 SHA256:iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
2026-04-21T19:33:51.207559+00:00 minitwit-app sshd[3928954]: Accepted publickey for root from 172.184.174.144 port 60417 ssh2: ED25519 SHA256:iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
2026-04-21T19:33:54.816885+00:00 minitwit-app sshd[3929032]: Accepted publickey for root from 172.184.174.144 port 60418 ssh2: ED25519 SHA256:iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
2026-04-21T20:02:38.599273+00:00 minitwit-app sshd[3929176]: Accepted publickey for root from 172.215.239.51 port 10368 ssh2: ED25519 SHA256:iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
2026-04-21T20:02:41.304723+00:00 minitwit-app sshd[3929230]: Accepted publickey for root from 172.215.239.51 port 10369 ssh2: ED25519 SHA256:iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
2026-04-21T20:02:44.231777+00:00 minitwit-app sshd[3929276]: Accepted publickey for root from 172.215.239.51 port 10370 ssh2: ED25519 SHA256:iVsayaUOt4pfhUW1gnZmrYyNnTMdM2O2nw67EkRqFEY
2026-04-21T20:21:53.910286+00:00 minitwit-app sshd[3929466]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T21:20:13.216405+00:00 minitwit-app sshd[3929856]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-21T23:18:21.881285+00:00 minitwit-app sshd[3930684]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-22T00:18:07.027139+00:00 minitwit-app sshd[3930910]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-22T01:18:08.099286+00:00 minitwit-app sshd[3931163]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-22T02:18:09.306229+00:00 minitwit-app sshd[3931377]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-22T03:18:09.216350+00:00 minitwit-app sshd[3931658]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-22T04:16:26.011311+00:00 minitwit-app sshd[3931782]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-22T05:16:19.474745+00:00 minitwit-app sshd[3931920]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-22T07:12:03.765287+00:00 minitwit-app sshd[3932388]: userauth_pubkey: signature algorithm ssh-rsa not in PubkeyAcceptedAlgorithms [preauth]
2026-04-22T07:54:38.670770+00:00 minitwit-app sshd[3932545]: Accepted publickey for root from 77.33.156.74 port 40598 ssh2: ED25519 SHA256:NKXnNQ5Lc7OylXJ40Pe96obZIbdK58Gzn0kTCCU9OCU
root@minitwit-app:/opt/monitoring# docker ps -a
CONTAINER ID   IMAGE                    COMMAND                  CREATED       STATUS       PORTS                                         NAMES
1104a9c2113a   grafana/loki:3.5.0       "/usr/bin/loki -conf…"   4 weeks ago   Up 2 weeks   0.0.0.0:3100->3100/tcp, [::]:3100->3100/tcp   loki
c72d0985ee84   prom/prometheus:v3.5.1   "/bin/prometheus --c…"   4 weeks ago   Up 3 weeks   0.0.0.0:9090->9090/tcp, [::]:9090->9090/tcp   prometheus
4c6ed725a5bd   grafana/grafana:12.1     "/run.sh"                4 weeks ago   Up 4 weeks   0.0.0.0:3000->3000/tcp, [::]:3000->3000/tcp   grafana
root@minitwit-app:/opt/monitoring# ls -la /tmp /root
/root:
total 56
drwx------  7 root root 4096 Apr 21 08:50 .
drwxr-xr-x 22 root root 4096 Feb 23 18:22 ..
-rw-------  1 root root 8925 Apr 21 13:43 .bash_history
-rw-r--r--  1 root root 3106 Apr 22  2024 .bashrc
drwx------  2 root root 4096 Feb 23 19:27 .cache
-rw-r--r--  1 root root    0 Feb 23 18:22 .cloud-locale-test.skip
drwx------  4 root root 4096 Apr 22 07:54 .config
drwx------  3 root root 4096 Mar 10 19:08 .docker
drwxr-xr-x  3 root root 4096 Mar 10 18:39 .local
-rw-r--r--  1 root root  161 Apr 22  2024 .profile
-rwxr-xr-x  1 root root  477 Mar 10 18:43 .setup-preprod-droplet.sh
drwx------  2 root root 4096 Mar 21 09:56 .ssh
-rw-r--r--  1 root root  185 Apr 22 07:22 .wget-hsts

/tmp:
total 36
drwxrwxrwt  9 root root 4096 Apr 22 02:11 .
drwxr-xr-x 22 root root 4096 Feb 23 18:22 ..
drwx------  2 root root 4096 Feb 23 18:22 snap-private-tmp
drwx------  3 root root 4096 Apr 15 06:32 systemd-private-df78a1d679ad4a0d92050f33a1e61484-ModemManager.service-xSIj1l
drwx------  3 root root 4096 Apr 15 06:32 systemd-private-df78a1d679ad4a0d92050f33a1e61484-fwupd.service-m0mG5y
drwx------  3 root root 4096 Apr 15 06:32 systemd-private-df78a1d679ad4a0d92050f33a1e61484-polkit.service-dTJcy6
drwx------  3 root root 4096 Feb 23 18:22 systemd-private-df78a1d679ad4a0d92050f33a1e61484-systemd-logind.service-4hmFyT
drwx------  3 root root 4096 Apr  9 06:59 systemd-private-df78a1d679ad4a0d92050f33a1e61484-systemd-resolved.service-34eHco
drwx------  3 root root 4096 Apr  9 06:59 systemd-private-df78a1d679ad4a0d92050f33a1e61484-systemd-timesyncd.service-JVS4pK
root@minitwit-app:/opt/monitoring# find / -name "*.sh" -newer /var/log/auth.log -not -path "*/proc/*" 2>/dev/null | head -20
root@minitwit-app:/opt/monitoring# cat /root/.setup-preprod-droplet.sh
#!/usr/bin/env bash
set -euo pipefail

# ── 1. Install Docker ──
curl -fsSL https://get.docker.com | sh
systemctl enable docker
systemctl start docker

# Verify
docker --version
docker compose version

# ── 2. Create the app directory (same structure as production) ──
mkdir -p /opt/itu-minitwit/data

# ── 3. Log in to Docker Hub so the droplet can pull your images ──
# Use your Docker Hub username/password (or a read-only access token)
docker login
root@minitwit-app:/opt/monitoring# ^C
root@minitwit-app:/opt/monitoring# cat /root/.bash_history
apt update
apt install -y docker.io docker-compose-v2 || apt install -y docker.io docker-compose-plugin
systemctl enable docker
systemctl start docker
docker --version
docker compose version
exit
cd minitwit/
docker compose up -d --build
docker compose ps
sudo tar -czf /opt/itu-minitwit/db_backup_$(date +%F-%H%M).tar.gz -C /opt/itu-minitwit data
ls -lh /opt/itu-minitwit/db_backup_*.tar.gz | tail -n 1
cd /opt
cd digitalocean/
ls
cd ..
ls
cd root/
ls
cd minitwit/
ls
exit
ls
cd minitwit/
ls
cd ..
rm -rf minitwit/
ls
# On the droplet:
mkdir -p ~/.ssh
echo "PASTE_YOUR_PUBLIC_KEY_HERE" >> ~/.ssh/authorized_keys
chmod 600 ~/.ssh/authorized_keys
ls
cd .ssh
ls
nano authorized_keys
..
cd .
cd ..
#!/usr/bin/env bash
set -euo pipefail
# ── 1. Install Docker ──
curl -fsSL https://get.docker.com | sh
cd /opt
cd /root
ls
nano .setup-preprod-droplet.sh
chmod +x .setup-preprod-droplet.sh
./.setup-preprod-droplet.sh
docker login
mkdir -p /opt/itu-minitwit/data
cd /opt/itu-minitwit/
ls
ssh-keygen -t ed25519 -C "preprod-deploy" -f ~/.ssh/minitwit_preprod
cat ~/.ssh/minitwit_preprod
exit
docker pull sebseb10/itu-minitwit:latest
curl -fsSL https://get.docker.com | sh
systemctl enable docker
systemctl start docker
docker --version
docker compose version
cd /opt
# Remove the empty directory we just made
rm -rf itu-minitwit
# Clone the repo
git clone https://github.com/sebseb10/ITU-MiniTwit.git itu-minitwit
cd itu-minitwit
export DOCKER_USERNAME=sebseb10
export TAG="latest"
chmod +x scripts/deploy.sh
bash scripts/deploy.sh
systemctl start docker
systemctl enable docker
docker ps
cd /opt/itu-minitwit
TAG=latest DOCKER_USERNAME=sebseb10 bash scripts/deploy.sh
exit
cd .ssh
ls
exit
cd .ssh
ls
ls -la ~/.ssh
cat ~/.ssh/authorized_keys
nano authorized_keys
nano minitwit_preprod
cat /root/.ssh/minitwit_preprod.pub
nano authorized_keys
cat /root/.ssh/minitwit_preprod.pub >> /root/.ssh/authorized_keys
chmod 600 /root/.ssh/authorized_keys
chmod 700 /root/.ssh
grep -F "$(cat /root/.ssh/minitwit_preprod.pub)" /root/.ssh/authorized_keys
cat /root/.ssh/minitwit_preprod
clear
docker ps --format "table {{.Names}}\t{{.Ports}}\t{{.Status}}"
sudo ss -ltnp | grep 5001
docker stop minitwit-chirp-web-1
docker rm minitwit-chirp-web-1
cd /opt/itu-minitwit
docker compose ps
docker ps --format "table {{.Names}}\t{{.Image}}\t{{.Ports}}\t{{.Status}}"
docker compose logs --tail=200 itu-minitwit
curl -i http://localhost:5001/
ss -ltnp | grep 5001
cat /opt/itu-minitwit/.env
docker exec -it itu-minitwit-itu-minitwit-1 printenv | grep ConnectionStrings
docker compose logs --tail=50 itu-minitwit
grep '^ConnectionStrings__DefaultConnection=' /opt/itu-minitwit/.env | cat -A
clear
cat /opt/itu-minitwit/docker-compose.yml
docker compose up
docker compose down
docker compose up -build
docker compose up
docker compose ps
docker ps -a --format "table {{.Names}}\t{{.Image}}\t{{.Status}}"
docker compose logs --tail=100 itu-minitwit
cd /opt/itu-minitwit/
ls
docker compose ps
docker compose logs --tail=100 itu-minitwit
clear
cd /opt/itu-minitwit
docker compose ps
docker compose logs --tail=100 itu-minitwit
docker compose port itu-minitwit 5001
curl -I http://localhost:5001
clear
ss -tulpn | grep 5001
grep '^ConnectionStrings__DefaultConnection=' /opt/itu-minitwit/.env
nc -vz 207.154.201.230 25060
nc -vz 207.154.201.230 25060
apt update && apt install -y netcat-openbsd
nc -vz 207.154.201.230 25060
docker compose --profile localdb up -d
grep '^ConnectionStrings__DefaultConnection=' /opt/itu-minitwit/.env
nc -vz 207.154.201.230 25060
cd /opt/itu-minitwit
docker compose restart itu-minitwit
docker compose logs --tail=100 itu-minitwit
nc -vz 207.154.201.230 25060
cat > /opt/itu-minitwit/.env <<'EOF'
DOCKER_USERNAME=sebseb10
TAG=16467b9f14c58d9bf67b00cd94ce14180e720c33
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5001
ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=chirp;Username=postgres;Password=postgres
EOF

cd /opt/itu-minitwit
docker compose --profile localdb up -d
docker compose logs --tail=100 itu-minitwit
grep '^ConnectionStrings__DefaultConnection=' /opt/itu-minitwit/.env
ls
rm -rm minitwit/
rm -rf minitwit/
ls
cd /opt
ls
rm -rf itu-minitwit/
ls
mkdir monitoring
docker --version
curl -fsSL https://get.docker.com | sh
curl -fsSL https://get.docker.com | sh
cd .ssh
ls
cat minitwit_preprod
ls -la /opt/monitoring/
cd /opt/monitoring/monitoring/
cat docker-compose.monitoring.yml
docker rm -f grafana
docker rm -f prometheus
cat /opt/monitoring/monitoring/prometheus/prometheus.yml
clear
cd /opt/monitoring/monitoring
docker compose -f docker-compose.monitoring.yml restart grafana
docker logs alloy --tail 50
docker ps
docker compose -f docker-compose.monitoring.yml restart grafana
curl -k https://itu-minitwit.duckdns.org/metrics | head -
curl -k https://itu-minitwit.duckdns.org/metrics | head -5
dig +short itu-minitwit.duckdns.org
exit
clear
dig +short itu-minitwit.duckdns.org
curl -k https://46.101.231.189/metrics -H "Host: itu-minitwit.duckdns.org" 2>&1 | head -5
tail -20 /var/log/nginx/access.log
curl -k https://itu-minitwit.duckdns.org/metrics | head -5
curl http://localhost:9090/api/v1/targets | python3 -m json.tool | head -30
cat /prometheus.yml
cd /opt/monitoring/monitoring/prometheus/
cat prometheus.yml
cd /opt/monitoring/monitoring
docker compose -f docker-compose.monitoring.yml restart prometheus
cd /nginx
cd ~/nginx
cd ..
ls
cd opt
ls
exit
docker ps
htop

kill -9 3772635
ls
cd .ssh/
ls
cat authorized_keys
cd
kill -9 3772635
file /tmp/mysql
rkhunter --check
# Recent auth attempts
journalctl -u ssh | grep "Accepted\|Failed" | tail -50
# Grafana query load — look for long-running or repeated queries
journalctl -u grafana-server | grep -i "query\|slow\|timeout" | tail -50
ps aux --sort=-%cpu | head -20
pkill -9 -u dnsmasq
usermod -L dnsmasq
ps aux --sort=-%cpu | head -20
ls
docker ps
ss -tulpn | grep 5432
# Most important — what network connections does it have?
ss -tulpn | grep $(pgrep -f /tmp/mysql)
# or
cat /proc/$(pgrep -f /tmp/mysql)/net/tcp
# Where is it connecting to?
nsenter -t $(pgrep -f /tmp/mysql) -n ss -tnp
# Check DNS lookups it's making
tcpdump -i any -n port 53 -l | grep -i mysql &
sleep 10 && kill %1docker ps
cd /opt/monitoring/
ls
cd monitoring/
ls
cat docker-compose.monitoring.yml
# Check when it was created and by whom
docker inspect chirp-postgres | grep -E "Created|StartedAt"
# Check what's inside it
docker exec chirp-postgres psql -U postgres -c "\l"   # list databases
docker exec chirp-postgres psql -U postgres -c "\du"  # list users
# Check its logs for connections
docker logs chirp-postgres 2>&1 | tail -50
# 1. Stop and remove the rogue postgres
docker stop chirp-postgres
docker rm chirp-postgres
# 2. Check your docker-compose.yml - postgres shouldn't be in it
cat docker-compose.yml | grep -A 20 postgres
# 3. Also check if the app droplet's postgres also has priv_esc user!
# SSH into your OTHER droplet and run:
docker exec <postgres-container> psql -U postgres -c "\du"
docker ps
docker kill 57a4ff44b40b
docker ps
exit
docker inspect grafana | grep -i network
docker inspect alloy | grep -A 20 "Networks"
docker logs alloy --tail 50
ls ~/
docker ps
find / -name "docker-compose*.yml" 2>/dev/null
find /opt/monitoring -name "*.alloy" 2>/dev/null
cat /opt/monitoring/monitoring/alloy/config.alloy
cat /opt/monitoring/docker-compose.yml
cat /opt/monitoring/monitoring/docker-compose.monitoring.yml
nano /opt/monitoring/docker-compose.yml
cd /opt/monitoring
docker compose up -d alloy
docker ps | grep alloy
docker inspect alloy | grep -A 30 "Networks"
ls
cat /opt/monitoring/docker-compose.yml | grep -A 20 "alloy"
cat /opt/monitoring/docker-compose.yml | grep -A 10 "^networks"
cat /opt/monitoring/docker-compose.yml | grep -A 20 "alloy"
cat /opt/monitoring/docker-compose.yml | grep -A 10 "^networks"
docker network ls
cd /opt/monitoring
docker compose up -d --force-recreate alloy
docker inspect alloy | grep -A 5 "monitoring_monitoring-network"
docker compose restart alloy
cat /opt/monitoring/monitoring/loki/loki-config.yml | grep -i "reject\|retention\|max_stream"
docker compose restart alloy
docker inspect alloy | grep -A 5 "monitoring_monitoring-network"
ps aux | grep /tmp/mysql
# Check if the bash watchdogs are still running
ps aux | grep dnsmasq
# Find the persistence NOW before it respawns
crontab -u dnsmasq -l 2>/dev/null
cat /var/spool/cron/crontabs/dnsmasq 2>/dev/null
ls -la /home/dnsmasq/.config/systemd/user/ 2>/dev/null
cat /home/dnsmasq/.bashrc 2>/dev/null
cat /home/dnsmasq/.profile 2>/dev/null
# Check /tmp for the dropper script
ls -la /tmp/
cat /opt/monitoring/monitoring/docker-compose.yml
cat /opt/monitoring/monitoring/docker-compose.monitoring.yml
docker stop chirp-postgres itu-minitwit-itu-minitwit-1
docker rm chirp-postgres itu-minitwit-itu-minitwit-1
find / -name "docker-compose*" 2>/dev/null
cat /opt/monitoring/docker-compose.yml
root@minitwit-app:/opt/monitoring# grep "dnsmasq\|mysql\|/tmp" /root/.bash_history | head -30
file /tmp/mysql
pkill -9 -u dnsmasq
usermod -L dnsmasq
ss -tulpn | grep $(pgrep -f /tmp/mysql)
cat /proc/$(pgrep -f /tmp/mysql)/net/tcp
nsenter -t $(pgrep -f /tmp/mysql) -n ss -tnp
tcpdump -i any -n port 53 -l | grep -i mysql &
ps aux | grep /tmp/mysql
ps aux | grep dnsmasq
crontab -u dnsmasq -l 2>/dev/null
cat /var/spool/cron/crontabs/dnsmasq 2>/dev/null
ls -la /home/dnsmasq/.config/systemd/user/ 2>/dev/null
cat /home/dnsmasq/.bashrc 2>/dev/null
cat /home/dnsmasq/.profile 2>/dev/null
# Check /tmp for the dropper script
ls -la /tmp/
root@minitwit-app:/opt/monitoring# last -F | grep -v "130.226\|37.97\|77.33" | head -20
root     pts/0        162.243.188.66   Sat Mar 21 09:51:59 2026 - Sat Mar 21 11:05:10 2026  (01:13)
reboot   system boot  6.8.0-71-generic Mon Feb 23 18:22:16 2026   still running

wtmp begins Mon Feb 23 18:22:16 2026
root@minitwit-app:/opt/monitoring# grep "Accepted\|Failed\|Invalid" /var/log/auth.log | grep -v "130.226\|37.97\|smaja\|anys\|toon\|pann" | head -50
2026-04-19T00:01:32.933043+00:00 minitwit-app sshd[1727812]: Invalid user sol from 80.94.92.184 port 34070
2026-04-19T00:04:23.789257+00:00 minitwit-app sshd[1742266]: Invalid user solana from 80.94.92.184 port 36700
2026-04-19T00:07:15.280556+00:00 minitwit-app sshd[1756221]: Invalid user solana from 80.94.92.184 port 39310
2026-04-19T00:10:04.579328+00:00 minitwit-app sshd[1769978]: Invalid user ubuntu from 80.94.92.184 port 41948
2026-04-19T00:12:53.975292+00:00 minitwit-app sshd[1783278]: Invalid user ubuntu from 80.94.92.184 port 44560
2026-04-19T00:15:40.635169+00:00 minitwit-app sshd[1796278]: Invalid user ubuntu from 80.94.92.184 port 47188
2026-04-19T00:18:26.978200+00:00 minitwit-app sshd[1808671]: Invalid user ubuntu from 80.94.92.184 port 49812
2026-04-19T00:23:57.041360+00:00 minitwit-app sshd[1834378]: Invalid user admin from 45.148.10.121 port 48008
2026-04-19T00:30:20.764804+00:00 minitwit-app sshd[1863951]: Invalid user user from 2.57.121.25 port 33596
2026-04-19T00:43:43.917045+00:00 minitwit-app sshd[1924962]: Invalid user admin from 200.126.105.149 port 7626
2026-04-19T00:56:16.872310+00:00 minitwit-app sshd[1980748]: Invalid user kimberlee from 213.209.159.159 port 30289
2026-04-19T01:03:41.150801+00:00 minitwit-app sshd[2011579]: Invalid user admin from 2.57.121.112 port 44258
2026-04-19T01:09:49.234617+00:00 minitwit-app sshd[2037363]: Invalid user AdminGPON from 45.148.10.121 port 45550
2026-04-19T01:12:45.637226+00:00 minitwit-app sshd[2049678]: Invalid user administrator from 142.248.80.104 port 58062
2026-04-19T01:37:52.512520+00:00 minitwit-app sshd[2159880]: Invalid user kt from 61.151.249.194 port 39804
2026-04-19T01:38:04.469285+00:00 minitwit-app sshd[2160916]: Invalid user n8n from 135.235.138.43 port 34688
2026-04-19T01:41:41.759380+00:00 minitwit-app sshd[2176332]: Invalid user ubuntu from 135.235.138.43 port 57054
2026-04-19T01:44:59.164876+00:00 minitwit-app sshd[2190866]: Invalid user soporte from 135.235.138.43 port 37366
2026-04-19T01:47:34.501629+00:00 minitwit-app sshd[2203362]: Invalid user user from 2.57.121.25 port 13137
2026-04-19T01:50:11.529892+00:00 minitwit-app sshd[2214847]: Invalid user sammy from 135.235.138.43 port 57174
2026-04-19T01:51:11.430213+00:00 minitwit-app sshd[2219209]: Invalid user 1234 from 193.46.255.86 port 19902
2026-04-19T01:51:53.059610+00:00 minitwit-app sshd[2222142]: Invalid user ftptest from 135.235.138.43 port 39992
2026-04-19T01:53:32.704894+00:00 minitwit-app sshd[2229633]: Invalid user ftpuser from 135.235.138.43 port 58690
2026-04-19T01:55:14.047810+00:00 minitwit-app sshd[2237419]: Invalid user tim from 135.235.138.43 port 43796
2026-04-19T01:56:11.459395+00:00 minitwit-app sshd[2241983]: Invalid user git from 61.151.249.194 port 57842
2026-04-19T01:56:55.164487+00:00 minitwit-app sshd[2245592]: Invalid user postgres from 135.235.138.43 port 56664
2026-04-19T01:57:20.459549+00:00 minitwit-app sshd[2247540]: Invalid user test8 from 61.151.249.194 port 46672
2026-04-19T01:58:34.347630+00:00 minitwit-app sshd[2253240]: Invalid user postgres from 135.235.138.43 port 35462
2026-04-19T02:00:17.011335+00:00 minitwit-app sshd[2261153]: Invalid user n8n from 135.235.138.43 port 38158
2026-04-19T02:03:48.969091+00:00 minitwit-app sshd[2277333]: Invalid user ftptest from 135.235.138.43 port 54980
2026-04-19T02:05:32.561841+00:00 minitwit-app sshd[2285104]: Invalid user steam from 135.235.138.43 port 41842
2026-04-19T02:09:00.956249+00:00 minitwit-app sshd[2301835]: Invalid user postgres from 135.235.138.43 port 60754
2026-04-19T02:09:45.901160+00:00 minitwit-app sshd[2305442]: Invalid user mackenna from 213.209.159.159 port 8373
2026-04-19T02:12:19.687303+00:00 minitwit-app sshd[2317318]: Invalid user mango from 135.235.138.43 port 39748
2026-04-19T02:14:03.840858+00:00 minitwit-app sshd[2325456]: Invalid user ftpuser from 135.235.138.43 port 47978
2026-04-19T02:17:47.442437+00:00 minitwit-app sshd[2342798]: Invalid user david from 61.151.249.194 port 43820
2026-04-19T02:18:11.725300+00:00 minitwit-app sshd[2344762]: Invalid user admin from 2.57.121.112 port 8795
2026-04-19T02:18:51.828987+00:00 minitwit-app sshd[2347855]: Invalid user pacs from 61.151.249.194 port 60910
2026-04-19T02:18:58.535754+00:00 minitwit-app sshd[2348460]: Invalid user user from 45.148.10.121 port 44506
2026-04-19T02:21:00.594056+00:00 minitwit-app sshd[2358002]: Invalid user ftptest from 135.235.138.43 port 41902
2026-04-19T02:44:16.204693+00:00 minitwit-app sshd[2468020]: Invalid user administrator from 142.248.80.104 port 41556
2026-04-19T03:04:33.739134+00:00 minitwit-app sshd[2564012]: Invalid user user from 2.57.121.25 port 20326
2026-04-19T03:22:40.583379+00:00 minitwit-app sshd[2640724]: Invalid user mallorie from 213.209.159.159 port 56078
2026-04-19T03:25:22.417264+00:00 minitwit-app sshd[2649137]: Invalid user ubnt from 45.148.10.121 port 48262
2026-04-19T03:32:49.085404+00:00 minitwit-app sshd[2682036]: Invalid user admin from 2.57.121.112 port 54739
2026-04-19T04:17:16.305310+00:00 minitwit-app sshd[2886981]: Invalid user administrator from 142.248.80.104 port 56760
2026-04-19T04:20:35.675501+00:00 minitwit-app sshd[2900918]: Invalid user a from 154.144.243.138 port 54534
2026-04-19T04:21:30.311348+00:00 minitwit-app sshd[2905044]: Invalid user user from 2.57.121.25 port 9845
2026-04-19T04:23:15.086822+00:00 minitwit-app sshd[2912323]: Invalid user  from 47.88.30.94 port 42950
2026-04-19T04:25:27.118795+00:00 minitwit-app sshd[2921510]: Invalid user test from 45.148.10.121 port 42128
root@minitwit-app:/opt/monitoring# grep "dnsmasq\|mysql\|/tmp" /root/.bash_history | head -30
file /tmp/mysql
pkill -9 -u dnsmasq
usermod -L dnsmasq
ss -tulpn | grep $(pgrep -f /tmp/mysql)
cat /proc/$(pgrep -f /tmp/mysql)/net/tcp
nsenter -t $(pgrep -f /tmp/mysql) -n ss -tnp
tcpdump -i any -n port 53 -l | grep -i mysql &
ps aux | grep /tmp/mysql
ps aux | grep dnsmasq
crontab -u dnsmasq -l 2>/dev/null
cat /var/spool/cron/crontabs/dnsmasq 2>/dev/null
ls -la /home/dnsmasq/.config/systemd/user/ 2>/dev/null
cat /home/dnsmasq/.bashrc 2>/dev/null
cat /home/dnsmasq/.profile 2>/dev/null
# Check /tmp for the dropper script
ls -la /tmp/
root@minitwit-app:/opt/monitoring# last -F | grep -v "130.226\|37.97\|77.33" | head -20
root     pts/0        162.243.188.66   Sat Mar 21 09:51:59 2026 - Sat Mar 21 11:05:10 2026  (01:13)
reboot   system boot  6.8.0-71-generic Mon Feb 23 18:22:16 2026   still running

wtmp begins Mon Feb 23 18:22:16 2026
root@minitwit-app:/opt/monitoring# 
```