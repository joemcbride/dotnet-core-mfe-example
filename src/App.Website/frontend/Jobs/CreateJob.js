import { gql, useMutation } from '@apollo/client'
import { useFormik } from 'formik'
import Header from '../components/Header'
import SubmitButton from '../components/SubmitButton'

const CREATE_JOB_MUTATION = gql`
  mutation CreateJob($dataFileOnly: Boolean!) {
    createJob(input: { dataFileOnly: $dataFileOnly }) {
      jobId
      status
      startedAtTimeUtc
    }
  }
`

export default function CreateJob() {
  const [createJob, { data, loading, error }] = useMutation(CREATE_JOB_MUTATION)

  const formik = useFormik({
    initialValues: {
      dataFileOnly: true,
    },
    onSubmit: async (values) => {
      console.log('form values', values)
      try {
        await createJob({
          variables: {
            dataFileOnly: values.dataFileOnly,
          },
        })
      } catch (e) {
        console.log(e)
      }
    },
  })

  if (loading) return 'Submitting...'
  if (error) return `Submission error! ${error.message}`

  if (data) {
    console.log('submit result', data)
  }

  return (
    <form onSubmit={formik.handleSubmit}>
      <Header>Create Job</Header>
      <div className="relative flex items-start border-b border-gray-900/10 pb-5">
        <div className="flex h-6 items-center">
          <input
            id="dataFileOnly"
            name="dataFileOnly"
            onChange={formik.handleChange}
            checked={formik.values.dataFileOnly}
            type="checkbox"
            className="h-4 w-4 rounded border-gray-300 text-indigo-600 focus:ring-indigo-600"
          />
        </div>
        <div className="ml-3 text-sm leading-6">
          <label htmlFor="dataFileOnly" className="font-medium text-gray-900">
            Create Data File Only
          </label>
        </div>
      </div>
      <div className="mt-6 flex items-center justify-end gap-x-6">
        <SubmitButton>Submit</SubmitButton>
      </div>
    </form>
  )
}
