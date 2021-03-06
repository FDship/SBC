﻿using SBC.Core;
using SBC.Cryptography;
using SBC.SmartContract;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SBC.Wallets
{
    public class KeyPair : IEquatable<KeyPair>
    {
        public readonly byte[] PrivateKey;
        public readonly Cryptography.ECC.ECPoint PublicKey;

        public UInt160 PublicKeyHash => PublicKey.EncodePoint(true).ToScriptHash();

        public KeyPair(byte[] privateKey)
        {
            if (privateKey.Length != 32 && privateKey.Length != 96 && privateKey.Length != 104)
                throw new ArgumentException();
            this.PrivateKey = new byte[32];
            Buffer.BlockCopy(privateKey, privateKey.Length - 32, PrivateKey, 0, 32);
            if (privateKey.Length == 32)
            {
                this.PublicKey = Cryptography.ECC.ECCurve.Secp256r1.G * privateKey;
            }
            else
            {
                this.PublicKey = Cryptography.ECC.ECPoint.FromBytes(privateKey, Cryptography.ECC.ECCurve.Secp256r1);
            }
#if NET47
            ProtectedMemory.Protect(PrivateKey, MemoryProtectionScope.SameProcess);
#endif
        }

        public IDisposable Decrypt()
        {
#if NET47
            return new ProtectedMemoryContext(PrivateKey, MemoryProtectionScope.SameProcess);
#else
            return new System.IO.MemoryStream(0);
#endif
        }

        public bool Equals(KeyPair other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return PublicKey.Equals(other.PublicKey);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as KeyPair);
        }

        public string Export()
        {
            using (Decrypt())
            {
                byte[] data = new byte[34];
                data[0] = 0x80;
                Buffer.BlockCopy(PrivateKey, 0, data, 1, 32);
                data[33] = 0x01;
                string wif = data.Base58CheckEncode();
                Array.Clear(data, 0, data.Length);
                return wif;
            }
        }

        public string Export(string passphrase, int N = 16384, int r = 8, int p = 8)
        {
            using (Decrypt())
            {
                //获取地址合约脚本哈希
                UInt160 script_hash = Contract.CreateSignatureRedeemScript(PublicKey).ToScriptHash();
                //获取地址
                string address = Wallet.ToAddress(script_hash);
                //获取地址摘要前四字节
                byte[] addresshash = Encoding.ASCII.GetBytes(address).Sha256().Sha256().Take(4).ToArray();
                //计算scrypt key
                byte[] derivedkey = SCrypt.DeriveKey(Encoding.UTF8.GetBytes(passphrase), addresshash, N, r, p, 64);
                byte[] derivedhalf1 = derivedkey.Take(32).ToArray();
                byte[] derivedhalf2 = derivedkey.Skip(32).ToArray();
                //aes加密
                byte[] encryptedkey = XOR(PrivateKey, derivedhalf1).AES256Encrypt(derivedhalf2);
                byte[] buffer = new byte[39];
                //校验位
                buffer[0] = 0x01;
                buffer[1] = 0x42;
                buffer[2] = 0xe0;
                //将地址摘要前四字节写入缓存
                Buffer.BlockCopy(addresshash, 0, buffer, 3, addresshash.Length);
                //密文写入缓存
                Buffer.BlockCopy(encryptedkey, 0, buffer, 7, encryptedkey.Length);
                //base58加密
                return buffer.Base58CheckEncode();
            }
        }

        public override int GetHashCode()
        {
            return PublicKey.GetHashCode();
        }

        public override string ToString()
        {
            return PublicKey.ToString();
        }

        private static byte[] XOR(byte[] x, byte[] y)
        {
            if (x.Length != y.Length) throw new ArgumentException();
            return x.Zip(y, (a, b) => (byte)(a ^ b)).ToArray();
        }
    }
}
