"use client";
import Image from "next/image";

interface PaymentSuccessModalProps {
  onClose: () => void;
}

export default function PaymentSuccessModal({
  onClose,
}: PaymentSuccessModalProps) {
  return (
    <div className="fixed inset-0 z-50">
      {/* overlay */}
      <div className="absolute inset-0 bg-black/40 backdrop-blur-sm"></div>

      {/* center modal */}
      <div className="relative z-10 flex items-center justify-center min-h-screen px-4">
        <div className="relative bg-white rounded-lg shadow-2xl w-full max-w-3xl px-6 md:px-12 py-10 space-y-8">
          {/* top-left back link */}
          <button
            onClick={onClose}
            className="absolute top-6 left-6 text-sm text-[#333740] hover:underline"
          >
            ← Continue exploring scripts
          </button>

          {/* card image */}
          <div className="flex justify-center mt-6">
            <Image
              src="/mastercard.png"
              alt="Card"
              width={120}
              height={80}
              className="rounded-md"
            />
          </div>

          {/* success icon */}
          <div className="flex justify-center">
            <div className="w-14 h-14 rounded-full bg-green-100 flex items-center justify-center">
              <span className="text-green-600 text-3xl">✓</span>
            </div>
          </div>

          {/* text */}
          <div className="text-center space-y-3">
            <h2 className="text-2xl font-semibold text-[#22242A]">
              Payment successful!!
            </h2>
            <p className="text-sm text-[#333740] max-w-xl mx-auto leading-snug">
              You’ve successfully secured this script. The writer has been
              notified and you now have 14 days to review the script and
              collaborate.
            </p>
          </div>

          {/* buttons */}
          <div className="flex flex-col md:flex-row gap-4 justify-center mt-4">
            <button className="flex-1 bg-[#810306] hover:bg-[#4d0000] text-white py-3 rounded-md text-sm font-medium">
              View synopsis
            </button>
            <button className="flex-1 border border-[#810306] text-[#810306] py-3 rounded-md text-sm font-medium flex items-center justify-center gap-2 hover:bg-[#fff5f5]">
              <Image
                src="/download-icon.svg"
                alt="Download"
                width={18}
                height={18}
              />
              Download script
            </button>
          </div>

          {/* close button */}
          <button
            onClick={onClose}
            className="absolute top-6 right-6 text-gray-400 hover:text-gray-600 text-xl"
            aria-label="Close"
          >
            ✕
          </button>
        </div>
      </div>
    </div>
  );
}
