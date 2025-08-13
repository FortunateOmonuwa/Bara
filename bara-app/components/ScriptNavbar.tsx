"use client";
import { useState } from "react";

/** lightweight inline icons so you don't need any deps */
const Divider = () => (
  <span className="h-6 w-px bg-gray-300 mx-3" aria-hidden="true" />
);
const UndoIcon = () => (
  <svg viewBox="0 0 24 24" className="w-4 h-4">
    <path
      d="M7 7H3m0 0v4m0-4l5 5a7 7 0 1 0 2-2"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.5"
      strokeLinecap="round"
      strokeLinejoin="round"
    />
  </svg>
);
const RedoIcon = () => (
  <svg viewBox="0 0 24 24" className="w-4 h-4">
    <path
      d="M17 7h4m0 0v4m0-4l-5 5a7 7 0 1 1-2-2"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.5"
      strokeLinecap="round"
      strokeLinejoin="round"
    />
  </svg>
);
const PencilIcon = () => (
  <svg viewBox="0 0 24 24" className="w-4 h-4">
    <path
      d="M15 3l6 6m-2 2l-6-6M4 20l5-1 9-9-4-4-9 9-1 5z"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.5"
      strokeLinecap="round"
      strokeLinejoin="round"
    />
  </svg>
);
const UnderlineIcon = () => (
  <svg viewBox="0 0 24 24" className="w-4 h-4">
    <path
      d="M6 4v6a6 6 0 1 0 12 0V4M4 20h16"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.5"
      strokeLinecap="round"
      strokeLinejoin="round"
    />
  </svg>
);
const CommentIcon = () => (
  <svg viewBox="0 0 24 24" className="w-4 h-4">
    <path
      d="M21 12a7 7 0 0 1-7 7H9l-4 3v-5a7 7 0 0 1 0-12h9a7 7 0 0 1 7 7z"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.5"
      strokeLinecap="round"
      strokeLinejoin="round"
    />
  </svg>
);
const MinusIcon = () => (
  <svg viewBox="0 0 24 24" className="w-4 h-4">
    <path
      d="M5 12h14"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
    />
  </svg>
);
const PlusIcon = () => (
  <svg viewBox="0 0 24 24" className="w-4 h-4">
    <path
      d="M12 5v14M5 12h14"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
    />
  </svg>
);

export default function ScriptNavbar() {
  const [tool, setTool] = useState<
    "highlight" | "underline" | "comment" | null
  >("highlight");
  const [page, setPage] = useState(1);
  const totalPages = 50;
  const [zoom, setZoom] = useState(60);

  return (
    <header className="w-full bg-white border-b border-gray-200">
      <div className="h-12 px-4 flex items-center">
        {/* filename */}
        <div className="text-sm font-medium text-gray-900">
          Broken Promise.pdf
        </div>

        <Divider />

        {/* undo/redo */}
        <div className="flex items-center gap-2">
          <button
            aria-label="Undo"
            className="p-1 rounded hover:bg-gray-100 text-gray-700"
          >
            <UndoIcon />
          </button>
          <button
            aria-label="Redo"
            className="p-1 rounded hover:bg-gray-100 text-gray-700"
          >
            <RedoIcon />
          </button>
        </div>

        <Divider />

        {/* tools: Highlight (active), Underline, Add comment */}
        <div className="flex items-center gap-3">
          <button
            onClick={() => setTool("highlight")}
            className={[
              "flex items-center gap-2 rounded px-3 py-1 text-sm border",
              tool === "highlight"
                ? "bg-rose-50 text-rose-700 border-rose-200"
                : "bg-transparent text-gray-700 border-transparent hover:bg-gray-100",
            ].join(" ")}
          >
            <span>Highlight</span>
            <PencilIcon />
          </button>

          <button
            onClick={() => setTool("underline")}
            className="flex items-center gap-2 px-2 py-1 text-sm text-gray-700 rounded hover:bg-gray-100"
          >
            <span>Underline</span>
            <UnderlineIcon />
          </button>

          <button
            onClick={() => setTool("comment")}
            className="flex items-center gap-2 px-2 py-1 text-sm text-gray-700 rounded hover:bg-gray-100"
          >
            <span>Add comment</span>
            <CommentIcon />
          </button>
        </div>

        <Divider />

        {/* Page 1 of 50 */}
        <div className="flex items-center gap-2 text-sm text-gray-800">
          <span>Page</span>
          <input
            type="number"
            value={page}
            onChange={(e) => {
              const v = Number(e.target.value || 1);
              setPage(Math.min(Math.max(1, v), totalPages));
            }}
            className="h-7 w-10 text-center border border-gray-300 rounded outline-none focus:ring-2 focus:ring-gray-200"
          />
          <span className="text-gray-600">of</span>
          <span className="text-gray-900">{totalPages}</span>
        </div>

        {/* spacer */}
        <div className="flex-1" />

        {/* Zoom: − [ 60% ] + */}
        <div className="flex items-center gap-2">
          <button
            aria-label="Zoom out"
            onClick={() => setZoom((z) => Math.max(10, z - 10))}
            className="p-1 rounded hover:bg-gray-100 text-gray-700"
          >
            <MinusIcon />
          </button>

          <div className="h-7 flex items-center rounded border border-gray-300 px-2 text-sm text-gray-900">
            {zoom}%
          </div>

          <button
            aria-label="Zoom in"
            onClick={() => setZoom((z) => Math.min(500, z + 10))}
            className="p-1 rounded hover:bg-gray-100 text-gray-700"
          >
            <PlusIcon />
          </button>
        </div>
      </div>
    </header>
  );
}
