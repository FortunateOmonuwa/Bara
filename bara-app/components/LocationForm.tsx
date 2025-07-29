"use client";

export default function LocationForm() {
  return (
    <div className="text-[#22242A] text-sm">
      <form>
        {/* Your form fields here */}
        <div className="mb-4">
          <label className="block mb-1 font-medium">City</label>
          <input
            type="text"
            className="w-full border rounded px-3 py-2"
            placeholder="Enter your city"
          />
        </div>
        <div className="mb-4">
          <label className="block mb-1 font-medium">Country</label>
          <input
            type="text"
            className="w-full border rounded px-3 py-2"
            placeholder="Enter your country"
          />
        </div>
      </form>
    </div>
  );
}
