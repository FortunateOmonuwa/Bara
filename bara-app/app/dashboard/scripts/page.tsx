"use client";

import Image from "next/image";
import { useEffect, useState } from "react";
import DashboardNavbar from "@/components/DashboardNavbar";
import WriterProfileCard from "@/components/WriterProfileCard";
import PaymentSuccessModal from "@/components/PaymentSuccessModal"; 
interface Script {
  id: string;
  title: string;
  price: number;
  imageUrl: string;
  logline: string;
  synopsis?: string;
  ipOwnedByProducer?: boolean;
}

export default function ScriptPage() {
  const [script, setScript] = useState<Script | null>(null);
  const [copied, setCopied] = useState(false);

  const [selectedMethod, setSelectedMethod] = useState<"wallet" | "card">(
    "card"
  );
  const walletBalance = 0;

  // modal state
  const [showPaymentSuccessModal, setShowPaymentSuccessModal] = useState(false);

  const scriptLink = "https://your-script-link.com";

  useEffect(() => {
    async function fetchScript() {
      const data: Script = {
        id: "1",
        title: "Broken Promise",
        price: 300000,
        imageUrl: "/flowery.png",
        logline:
          "A desperate journalist uncovers a hidden AI network controlling world events and must race against time to expose the truth before becoming its next target.",
        ipOwnedByProducer: true,
      };
      setScript(data);
    }
    fetchScript();
  }, []);

  const handleCopy = async () => {
    await navigator.clipboard.writeText(scriptLink);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  const handlePayment = () => {
    // here you would call your API for payment
    setShowPaymentSuccessModal(true);
  };

  if (!script) {
    return (
      <main className="min-h-screen bg-white flex items-center justify-center">
        <p className="text-gray-500">Loading script…</p>
      </main>
    );
  }

  const walletDisabled = walletBalance < script.price;

  return (
    <main className="min-h-screen bg-white overflow-x-hidden relative">
      <DashboardNavbar />

      <div className="max-w-6xl mx-auto px-4 py-4 grid grid-cols-1 lg:grid-cols-12 gap-6">
        {/* LEFT SIDE */}
        <div className="lg:col-span-3 space-y-4">
          <h1 className="text-xl font-semibold text-[#22242A] break-words">
            {script.title}
          </h1>

          <Image
            src={script.imageUrl}
            alt={script.title}
            width={400}
            height={300}
            className="w-full h-auto rounded-md object-cover"
          />

          <p className="text-lg font-semibold text-[#22242A]">
            ₦{script.price.toLocaleString()}
          </p>

          <button className="w-full bg-[#FFEDEE] text-[#810306] text-sm font-semibold py-3 rounded-md">
            <span className="flex items-center gap-2 justify-start pl-6">
              <Image src="/heart.png" alt="Save" width={16} height={16} />
              Save this script
            </span>
          </button>

          <hr className="border-t border-[#ABADB2] my-2" />

          {/* Copy script section */}
          <div>
            <p className="text-sm text-[#333740] mb-1 font-medium">
              Copy this script
            </p>
            <div className="relative inline-flex items-center">
              <button
                onClick={handleCopy}
                className="p-1 hover:opacity-80"
                aria-label="Copy link"
              >
                <Image src="/copy.png" alt="Copy" width={20} height={20} />
              </button>

              {copied && (
                <span className="absolute left-1/2 -translate-x-1/2 top-full mt-1 text-xs text-[#0DA500] whitespace-nowrap">
                  Copied!
                </span>
              )}
            </div>
          </div>
        </div>

        {/* MIDDLE SECTION */}
        <div className="lg:col-span-6 space-y-4">
          <div className="bg-white border border-[#ABADB2] rounded-md p-4 space-y-4">
            {/* Logline Header */}
            <div className="flex items-center justify-between">
              <h2 className="font-semibold text-lg text-[#22242A]">Logline</h2>
              {script.ipOwnedByProducer && (
                <span className="flex items-center text-[11px] font-medium text-[#BF4E00] bg-[#FFD9BF] border border-[#BF4E00] px-2 py-1 rounded">
                  <Image
                    src="/info.png"
                    alt="Info"
                    width={14}
                    height={14}
                    className="mr-1"
                  />
                  IP owned by producer
                </span>
              )}
            </div>

            <p className="text-sm text-[#333740] leading-snug">
              {script.logline}
            </p>

            <div className="flex gap-2">
              <button className="flex-1 py-3 bg-[#F5F5F5] rounded-md text-sm font-medium text-[#858990] flex items-center justify-center gap-2 border border-[#E5E7EB]">
                <Image src="/lock.png" alt="Synopsis" width={16} height={16} />
                Synopsis
              </button>
              <button className="flex-1 py-3 bg-[#F5F5F5] rounded-md text-sm font-medium text-[#858990] flex items-center justify-center gap-2 border border-[#E5E7EB]">
                <Image src="/lock.png" alt="Script" width={16} height={16} />
                Script
              </button>
            </div>

            <hr className="border-t border-[#E5E7EB]" />

            {/* Payment methods */}
            <div className="space-y-3">
              <h3 className="font-semibold text-sm text-[#22242A]">
                Payment methods
              </h3>

              {/* Wallet Option */}
              <div className="flex items-center justify-between rounded-md py-3 px-3 bg-[#F5F5F5]">
                <div className="flex items-center gap-3">
                  <Image
                    src="/wallet.png"
                    alt="Wallet"
                    width={24}
                    height={24}
                  />
                  <div className="flex flex-col leading-tight">
                    <span className="text-sm font-medium text-[#333740]">
                      ₦{walletBalance}
                    </span>
                    <span className="text-[11px] text-[#858990]">Wallet</span>
                  </div>
                </div>
                <input
                  type="radio"
                  name="pay"
                  value="wallet"
                  disabled={walletDisabled}
                  checked={selectedMethod === "wallet"}
                  onChange={() => setSelectedMethod("wallet")}
                  className="accent-[#800000]"
                />
              </div>

              {/* Card Option */}
              <div className="bg-[#F5F5F5] rounded-md">
                <div className="flex items-center justify-between py-3 px-3">
                  <div className="flex items-center gap-2">
                    <Image
                      src="/mastercard.png"
                      alt="Card"
                      width={20}
                      height={20}
                    />
                    <span className="text-sm text-[#333740]">****1243</span>
                  </div>
                  <input
                    type="radio"
                    name="pay"
                    value="card"
                    checked={selectedMethod === "card"}
                    onChange={() => setSelectedMethod("card")}
                    className="accent-[#800000]"
                  />
                </div>
                <div>
                  <button className="w-full text-left text-xs font-medium text-[#810306] py-3 px-3">
                    + Add new card
                  </button>
                </div>
              </div>

              {/* Payment Button */}
              <button
                onClick={handlePayment}
                className="w-full bg-[#800000] text-white py-3 rounded-md text-sm font-medium hover:bg-[#4d0000] transition"
              >
                Make payment
              </button>

              {/* NDA Agreement */}
              <div className="flex items-start gap-2 mt-2">
                <input type="checkbox" className="mt-1 accent-[#800000]" />
                <p className="text-[11px] leading-snug text-[#333740]">
                  By checking this box, you agree to the Non‑Disclosure
                  Agreement, committing not to share, misuse, or reproduce the
                  information in this synopsis. Your payment will be securely
                  held by Bara. You have 14 days from today to review the
                  script, engage with the writer, and confirm the order.
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* RIGHT SIDE */}
        <div className="lg:col-span-3">
          <WriterProfileCard
            name="Timothy Edwards"
            bio="Award-winning writer and motivational speaker. Worked with a lot of top brands."
            profileImage="/writer.png"
            portfolioLink="https://timothy-edwards.com/works"
            onViewProfile={() => {
              console.log("View profile clicked!");
            }}
          />
        </div>
      </div>

      {/* Payment success modal */}
      {showPaymentSuccessModal && (
        <PaymentSuccessModal
          onClose={() => setShowPaymentSuccessModal(false)}
        />
      )}
    </main>
  );
}
