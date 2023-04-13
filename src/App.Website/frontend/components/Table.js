function Table({ children }) {
  return (
    <table className="min-w-full divide-y divide-gray-300">{children}</table>
  )
}

function Header({ children }) {
  return (
    <thead className="bg-gray-50">
      <tr>{children}</tr>
    </thead>
  )
}

function HeaderRow({ children }) {
  return (
    <th className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900">
      {children}
    </th>
  )
}

function Body({ children }) {
  return <tbody className="divide-y divide-gray-200 bg-white">{children}</tbody>
}

function Row({ children }) {
  return <tr>{children}</tr>
}

function Column({ children }) {
  return (
    <td className="whitespace-nowrap px-3 py-3 text-sm text-gray-500">
      {children}
    </td>
  )
}

Table.Header = Header
Table.HeaderRow = HeaderRow
Table.Body = Body
Table.Row = Row
Table.Column = Column

export default Table
